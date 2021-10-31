using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNetCoreMastersTodoList.Api.Filters
{
    public class RequestIdValidationAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var routeValues = context.HttpContext.Request.RouteValues;
            object strId;

            var hasQueryId = context.HttpContext.Request.QueryString.Value.Contains("id");
            var hasPathId = routeValues.TryGetValue("id", out strId);

            if (hasQueryId
                || hasPathId)
            {
                int id = -1;
                strId = hasQueryId ? context.HttpContext.Request.Query["id"] : strId;
                var isValidId = int.TryParse(strId.ToString(), out id);
                if (!isValidId)
                {
                    context.HttpContext.Response.StatusCode = 400;
                    return;
                }
            }

            await next();
            return;
        }
    }
}
