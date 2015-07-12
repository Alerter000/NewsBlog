using NewsBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsBlog.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(NewsSearcher target)
        {
            if (Request.IsAjaxRequest())
            {
                if (target.Find())
                    return PartialView("_GridForNews", target.Results);
                else
                    return Content("Ничего не найдено");
            }
            return View();
        }

    }
}
