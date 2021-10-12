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
    public class PerformanceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.HttpContext.Items["StartTime"] = DateTime.UtcNow;
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            DateTime startTime = (DateTime)context.HttpContext.Items["StartTime"];
            double elapsedTime = (DateTime.UtcNow - startTime).TotalSeconds;
            Console.WriteLine($"Elapsed time: {elapsedTime} sec");
        }
    }
}
