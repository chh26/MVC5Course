using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //TODO:ALSFJ;DKJL;
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult test()
        {
            return View();
        }

        public ActionResult NewIndex()
        {
            return View();
        }

        [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error2")]
        public ActionResult ErrorTest(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("參數錯誤");
            }
                throw new InvalidOperationException("操作錯誤");
            return View();
        }
    }
}