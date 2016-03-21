using MVC5Course.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class TestController : Controller
    {
        FabricsEntities db = new FabricsEntities();
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EDE()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EDE(EDEViewModel data)
        {
            return View(data);
        }

        public ActionResult CreateProduct()
        {
            Product product = new Product
            {
                ProductName = "EDE",
                Active = true,
                Price = 10000,
                Stock = 600

            };

            db.Product.Add(product);
            db.SaveChanges();

            return View(product);
        }

        public ActionResult ReadProduct(bool? Active)
        {
            var data = db.Product
                .Where(p => p.ProductId > 1550)
                .OrderByDescending(p => p.Price).AsQueryable();
            if (Active.HasValue)
            {
                data = data.Where(p => p.Active == Active);
            }
            return View(data);
        }

        public ActionResult OneProduct(int Id)
        {
            var data = db.Product.Find(Id);
            //var data = db.Product.FirstOrDefault(p => p.ProductId == Id);

            return View(data);
        }

        public ActionResult UpdateProduct(int Id)
        {
            var data = db.Product.Find(Id);
            if (data == null)
            {
                return HttpNotFound();
            }
            data.Price = data.Price * 2;
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                foreach (var entityError in e.EntityValidationErrors)
                {
                    foreach (var err in entityError.ValidationErrors)
                    {
                        return Content(err.PropertyName + " :" + err.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("ReadProduct");
        }

        public ActionResult DeleteProduct(int Id)
        {
            var data = db.Product.Find(Id);
            //因為Product有設定FK給OrderLind，若要刪除的Product有被OrderLine，刪除時就會發生錯誤
            //所以需要先處理OrderLine的資料刪除，以下為刪除Product有對應到OrderLine時的寫法
            //基本寫法
            foreach (var item in data.OrderLine.ToList())
            {
                db.OrderLine.Remove(item);
            }


            //進階寫法
            //db.OrderLine.RemoveRange(data.OrderLine);

            //ExecuteSqlCommand條件可用參數的方式，ex. @P0
            //db.Database.ExecuteSqlCommand(@"DELETE FROM dbo.OrderLine WHERE Product = @P0", Id);

            db.Product.Remove(data);
            db.SaveChanges();
            return RedirectToAction("ReadProduct");
        }

        public ActionResult ProductView()
        {
            //如Entity轉成SQL語法如有效能議題，可以用db.Database自行寫SQL，
            //因為如果Entity組太多，產生出來的SQL Script有可能會很多很亂很怪。
            //db.Database此寫法不會有導覽屬性
            var data = db.Database.SqlQuery<ProductViewModel>
                (@"SELECT * FROM dbo.Product WHERE Active = @P0 AND ProductName like @P1"
                ,true,"%White%");
            return View(data);
        }

        public ActionResult ProductSP()
        {
            var data = db.GetProduct(true, "%White%");
            return View(data);
        }
    }
}