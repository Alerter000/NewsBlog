using NewsBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsBlog.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            
            IEnumerable<News> news = null;
            //IEnumerable<User> users = null;
            using (UnitOfWork unit = new UnitOfWork())
            {
                NewsRepository newsRep = new NewsRepository(unit.DataContext);
                news = newsRep.GetAllNews().OrderBy(n => n.DatePublication).Reverse();               
            }
            return View(news);
        }

        public ActionResult Edit(Guid id)
        {
            if (Session["LoginUserID"] != null)
            {
                News news;
                using (UnitOfWork unit = new UnitOfWork())
                {
                    NewsRepository newsRep = new NewsRepository(unit.DataContext);
                    news = newsRep.GetNewsById(id);
                    if (UserValidator.IsValid(Session["LoginUserID"].ToString(), news.Owner))
                    {
                        Session["NewsId"] = id;
                        return View(news);
                    }
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Edit(News news, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid && Session["LoginUserID"] != null)
            {
                byte[] file = null;
                if (upload != null)
                    file = ConverterFileToBytes.Convert(upload);
                using (UnitOfWork unit = new UnitOfWork())
                {
                    NewsService newsServ = new NewsService(unit.DataContext);
                    NewsRepository newsRep = new NewsRepository(unit.DataContext);
                    UserRepository userRep = new UserRepository(unit.DataContext);
                    CommentRepository comRep = new CommentRepository(unit.DataContext);
                    news.NewsId = new Guid(Session["NewsId"].ToString());
                    news.Image = file;
                    news.DatePublication = newsRep.GetNewsById(news.NewsId).DatePublication;
                    news.Owner = userRep.GetUserById(new Guid(Session["LoginUserID"].ToString()));
                    news.Comments = comRep.GetCommentsByPublicationId(new Guid(Session["LoginUserID"].ToString())).ToList();
                    newsServ.Edit(news);
                    unit.Commit();
                }
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }

        public ActionResult Delete(Guid id)
        {
            if (Session["LoginUserID"] != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    NewsRepository newsRep = new NewsRepository(unit.DataContext);
                    User user = newsRep.GetNewsById(id).Owner;
                    if (UserValidator.IsValid(Session["LoginUserID"].ToString(), user))
                    {
                        NewsService newsServ = new NewsService(unit.DataContext);
                        newsServ.Delete(id);
                        unit.Commit();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UserRepository userRep = new UserRepository(unit.DataContext);
                    var authUser = userRep.GetUserByNameAndPass(user.Name, user.Password);
                    if (authUser != null)
                    {
                        Session["LoginUserID"] = authUser.UserId.ToString();
                        Session["LogedUserName"] = authUser.Name;
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(user);
        }


        public ActionResult AfterLogin()
        {
            if (Session["LoginUserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult LogOff()
        {
            Session["LoginUserID"] = null;
            Session["LogedUserName"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UserRepository userRep = new UserRepository(unit.DataContext);
                    User u = userRep.GetUserByName(user.Name);
                    if (u != null)
                    {
                        ViewBag.Message = "Пользователь с таким именем уже есть";
                        return View(user); 
                    }
                    UserService userServ = new UserService(unit.DataContext);
                    userServ.Create(user.Name, user.Password, user.EMail);
                    unit.Commit();
                    Session["LoginUserID"] = user.UserId;
                    Session["LogedUserName"] = user.Name;
                    ModelState.Clear();
                    user = null;
                    ViewBag.Message = "Регистрация успешно завершена";
                    return RedirectToAction("Index");
                }

            }
            return View(user);
        }

        

        public ActionResult AddNews()
        {
            if (Session["LoginUserID"] != null)
                return View();
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddNews(News news, HttpPostedFileBase upload)
        {
            byte[] file = null;
            if (ModelState.IsValid)
            {
                if (upload != null)
                {
                    file = ConverterFileToBytes.Convert(upload);
                }
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UserRepository userRep = new UserRepository(unit.DataContext);
                    User user = userRep.GetUserById(new Guid(Session["LoginUserID"].ToString()));
                    if (user != null)
                    {
                        NewsService newServ = new NewsService(unit.DataContext);
                        newServ.Create(user, news.Title, news.Content, news.Category, file);
                        unit.Commit();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult ReadMore(Guid id)
        {
            NewsAndComments newsAndComms = new NewsAndComments();
            IEnumerable<Comment> comments = null;
            using (UnitOfWork unit = new UnitOfWork())
            {
                NewsRepository newsRep = new NewsRepository(unit.DataContext);
                News news = newsRep.GetNewsById(id);
                CommentRepository comRep = new CommentRepository(unit.DataContext);
                comments = comRep.GetCommentsByPublicationId(id);
                newsAndComms.News = news;
                newsAndComms.Comments = comments;
                Session["NewsId"] = news.NewsId;
            }

            return View(newsAndComms);
        }

        public ActionResult AddComment(NewsAndComments comments)
        {
            if (Session["LoginUserID"] != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UserRepository userRep = new UserRepository(unit.DataContext);
                    NewsRepository newsRep = new NewsRepository(unit.DataContext);
                    CommentRepository comRep = new CommentRepository(unit.DataContext);
                    //News news = newsRep.GetNewsById(id);
                    User user = userRep.GetUserById(new Guid((Session["LoginUserID"]).ToString()));
                    CommentService comServ = new CommentService(unit.DataContext);
                    comments.News = newsRep.GetNewsById(new Guid((Session["NewsId"]).ToString()));
                    //comments.News.Comments = comRep.GetCommentsByPublicationId(new Guid((Session["NewsId"]).ToString())).ToList();
                    //comments.News.Owner = user;
                    comServ.Create(user, comments.News, comments.CurrentComment);
                    unit.Commit();
                    comments.Comments = newsRep.GetNewsById(comments.News.NewsId).Comments;
                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GridForComments", comments);
            }
            return RedirectToAction("ReadMore/" + comments.News.NewsId.ToString());
        }
        
        [HttpPost]
        public ActionResult DeleteComm(Guid id)
        {
            if (Session["LoginUserID"] != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    CommentRepository comRep = new CommentRepository(unit.DataContext);
                    CommentService comServ = new CommentService(unit.DataContext);
                    NewsAndComments newsAndComments = new NewsAndComments();
                    Guid newsId = new Guid(comRep.GetCommentById(id).Publication.NewsId.ToString());
                    newsAndComments.News = comRep.GetCommentById(id).Publication;
                    
                    if (UserValidator.IsValid(Session["LoginUserID"].ToString(), comRep.GetCommentById(id).Publication.Owner))
                    {
                        comServ.Delete(id);
                        unit.Commit();
                    }

                    newsAndComments.Comments = comRep.GetCommentsByPublicationId(newsId);                    
                    
                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("_GridForComments", newsAndComments);
                    }
                    else
                    {
                        return View("ReadMore", newsAndComments);
                    }
                }
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult SelectCategory(String category)
        {
            if (ModelState.IsValid && Request.IsAjaxRequest())
            {
                IEnumerable<News> news = null;
                using (UnitOfWork unit = new UnitOfWork())
                {
                    NewsRepository newsRep = new NewsRepository(unit.DataContext);
                    news = newsRep.GetAllNews().OrderBy(n => n.DatePublication).Reverse();
                    if (category != "Все")
                        news = news.Where(n => n.Category.Equals(category)).OrderBy(n => n.DatePublication).Reverse();
                }
                return PartialView("_GridForNews", news);
            }
            return View("Index");
        }
    }
}
