using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    [計算Action執行時間]//動作過濾器，可在執行動作的前後插入事件。
    [Authorize]
    public class ProductsController : BaseController
    {
        // GET: Products
        public ActionResult Index()
        {
            var data = repoProduct.All().Take(5);
            return View(data);
        }

        [HttpPost]
        public ActionResult Index(IList<BatchUpdateProduct> productList)
        {
            // 在 C# 中讀取資料的方式
            // data[0].Price

            // 原本在 View 顯示的每一筆欄位名稱
            // item.Price
            // item.Price

            // 調整為多筆資料繫結可以接到資料的欄位名稱
            // name="data[0].Price"
            // name="data[1].Price"
            if (ModelState.IsValid)
            {
                foreach (var item in productList)
                {
                    var product = repoProduct.Find(item.ProductId);
                    product.Price = item.Price;
                    product.Active = item.Active;
                    product.Stock = item.Stock;
                }

                repoProduct.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewData.Model = repoProduct.All().Take(5);
            return View();

        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                repoProduct.Add(product);
                repoProduct.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        //↑為預先驗證，product(model)物件只會包含 Include裡的屬性，
        //所以當Include裡的屬性action被拿掉時[Bind(Include = "ProductId,ProductName,Price,Stock")
        //此時product(model)物件裡沒有form回傳的action，
        //所以product(model)物件裡的action會為預設值false
        public ActionResult Edit(int id, FormCollection form)
        {
            //為預防上述資料綁定錯誤，可用
            //modelview直接綁定form要更新資料
            //或用TryUpdateModel（這個動作就是在modelbinding）延遲驗證方式，先帶出資料，再直接更新想更新的屬性，
            //寫法如下。
            var product = repoProduct.Find(id);
            if (TryUpdateModel(product, new string[] {
                "ProductId","ProductName","Price","Active","Stock"}))
            {
                var dbProduct = repoProduct.UnitOfWork.Context;

                dbProduct.Entry(product).State = EntityState.Modified;
                dbProduct.SaveChanges();

                TempData["msg"] = product.ProductName + " 更新成功。";
                //db.Entry(product).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repoProduct.Find(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product product = repoProduct.Find(id);

                repoProduct.Delete(product);
                repoProduct.UnitOfWork.Commit();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {

                foreach (var entityError in e.EntityValidationErrors)
                {
                    foreach (var err in entityError.ValidationErrors)
                    {
                        return Content(err.PropertyName + " :" + err.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                //foreach (var err in ex.)
                //{
                //    return Content(err.PropertyName + " :" + err.ErrorMessage);
                //}

                return Content(ex.InnerException.ToString());
            }
            
            //db.Product.Remove(product);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoProduct.UnitOfWork.Context.Dispose();
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
