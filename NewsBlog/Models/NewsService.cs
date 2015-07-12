using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class NewsService
    {
        private const String ERROR_OPEN_DATABASE = "Error open table UserFiles in database";
        private const String ERROR_CREATE_NEWS = "Error create file in database";
        private const String ERROR_DELETE_NEWS = "Error delete file in database";
        private const String ERROR_UPDATE_NEWS = "Error update file in database";
        public NewsService(NewsBlogDatabase db)
        {
            if (db == null)
                throw new ArgumentException(ERROR_OPEN_DATABASE);
            _database = db;
        }
        public News Create(User user, String title, String content, String category, byte[] image = null)
        {
            if (String.IsNullOrEmpty(title) || String.IsNullOrEmpty(content) || user == null)
                throw new ArgumentException(ERROR_CREATE_NEWS);
            News news = new News()
            {
                Title = title,
                Content = content,
                Owner = user,
                Category = category,
                Image = image,
                DatePublication = DateTime.Now,
                NewsId = Guid.NewGuid()
            };
            _database.News.Add(news);
            return news;
        }

        public News Create(News news)
        {
            if (news == null)
                throw new ArgumentException(ERROR_CREATE_NEWS);
            _database.News.Add(news);
            return news;
        }

        public void Edit(News news)
        {
            News newsInDb = _database.News.Find(news.NewsId);
            if (newsInDb == null)
            {
                Create(news);
            }
            else
            {
                _database.Entry(newsInDb).CurrentValues.SetValues(news);
                _database.Entry(newsInDb).State = System.Data.Entity.EntityState.Modified;
            }
        }
        public void Delete(News news)
        {
            if (news == null)
                throw new ArgumentException(ERROR_DELETE_NEWS);
            _database.News.Remove(news);
        }

        public void Delete(Guid id)
        {
            NewsRepository rep = new NewsRepository(_database);
            News news = rep.GetNewsById(id);
            CommentRepository comRep = new CommentRepository(_database);
            if (news == null)
                throw new ArgumentException(ERROR_DELETE_NEWS);
            IEnumerable<Comment> comments = comRep.GetCommentsByPublicationId(id);
            if (comments != null)
                foreach (var comment in comments)
                    _database.Comments.Remove(comment);
            _database.News.Remove(news);
        }

        private readonly NewsBlogDatabase _database;
    }
}