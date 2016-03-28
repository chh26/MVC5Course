using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ARController : BaseController
    {
        // GET: AR
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialViewTest()
        {
            //不載入Layout
            return PartialView("Index");
        }

        public ActionResult FileTest(int? dl)
        {
            if (dl.HasValue && dl.Value == 1)
            {
                return File(Server.MapPath("~/Content/one-piece.jpg"), "image/jpeg","OnePiece.jpg");
            }
            return File(Server.MapPath("~/Content/one-piece.jpg"), "image/jpeg");
        }

        public ActionResult JsonTest(int id)
        {
            repoProduct.UnitOfWork.Context.Configuration.LazyLoadingEnabled = false;
            var product = repoProduct.Find(id);
            return Json(product,JsonRequestBehavior.AllowGet);
        }
    }
}