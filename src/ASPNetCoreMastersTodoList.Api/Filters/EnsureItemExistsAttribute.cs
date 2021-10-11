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
        private readonly IItemService _itemService;

        public EnsureItemExistsFilter(IItemService itemService)
        {
            this._itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool hasId = context.ActionArguments.TryGetValue("id", out object itemIdObj);

            if (hasId)
            {
                if (!_itemService.ItemExists((int)itemIdObj))
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
