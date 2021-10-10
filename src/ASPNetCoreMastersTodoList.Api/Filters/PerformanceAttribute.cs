using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.Filters
{
    /// <summary>
    /// Keep this if need to implement own filter type. Remove if the current approach of inheriting ActionFilterAttribute is correct.
    /// </summary>
    public class PerformanceFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["StartTime"] = DateTime.UtcNow;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            DateTime startTime = (DateTime)context.HttpContext.Items["StartTime"];
            double elapsedTime = (DateTime.UtcNow - startTime).TotalSeconds;
            context.HttpContext.Response.Headers.Add("ElapsedTime", elapsedTime.ToString() + " sec");
        }
    }

    //public class PerformanceAttribute : TypeFilterAttribute
    //{
    //    public PerformanceAttribute() : base(typeof(PerformanceFilter))
    //    {
    //    }
    //    //public override void OnActionExecuting(ActionExecutingContext context)
    //    //{
    //    //    context.HttpContext.Items["StartTime"] = DateTime.UtcNow;
    //    //    base.OnActionExecuting(context);
    //    //}

    //    //public override void OnActionExecuted(ActionExecutedContext context)
    //    //{
    //    //    base.OnActionExecuted(context);
    //    //    DateTime startTime = (DateTime)context.HttpContext.Items["StartTime"];
    //    //    System.Diagnostics.Debug.WriteLine(context.HttpContext.Items["ActionName"].ToString() + "-" + (DateTime.UtcNow - startTime).TotalMilliseconds);
    //    //}

    //    //public override void OnResultExecuted(ResultExecutedContext context)
    //    //{
    //    //    base.OnResultExecuted(context);
    //    //}
    //}    

    public class PerformanceAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["StartTime"] = DateTime.UtcNow;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            DateTime startTime = (DateTime)context.HttpContext.Items["StartTime"];
            double elapsedTime = (DateTime.UtcNow - startTime).TotalSeconds;

            context.HttpContext.Response.Headers.Add("Elapsed-Time", elapsedTime.ToString() + " sec");
        }
    }
}
