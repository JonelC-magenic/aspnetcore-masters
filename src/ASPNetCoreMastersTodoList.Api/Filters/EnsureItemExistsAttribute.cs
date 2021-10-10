using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.Filters
{
    public class EnsureItemExistsFilter : IActionFilter
    {
        public readonly IItemService itemService;

        public EnsureItemExistsFilter(IItemService itemService)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var itemId = (int)context.ActionArguments["id"];

            if (!itemService.ItemExists(itemId))
            {
                context.Result = new NotFoundResult();
            }
        }
    }
    public class EnsureItemExistsAttribute : TypeFilterAttribute
    {
        public EnsureItemExistsAttribute() : base(typeof(EnsureItemExistsFilter))
        {
        }
    }
}
