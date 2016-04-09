using System;
using System.Web.Mvc;

namespace MVC5Course.Models
{
    public class 計算Action執行時間Attribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.dtStart = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.dtEnd = DateTime.Now;
            var actionTimeSpen = filterContext.Controller.ViewBag.dtEnd -
                filterContext.Controller.ViewBag.dtStart;
            filterContext.Controller.ViewBag.actionTimeSpen = actionTimeSpen;
            base.OnActionExecuted(filterContext);
        }
    }
}