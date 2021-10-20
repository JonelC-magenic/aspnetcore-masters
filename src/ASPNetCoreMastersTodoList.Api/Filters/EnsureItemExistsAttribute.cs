using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public EnsureItemExistsFilter(IItemService itemService, ILogger<EnsureItemExistsFilter> logger)
        {
            this._itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool hasId = context.ActionArguments.TryGetValue("id", out object itemIdObj);

            if (hasId && !_itemService.ItemExists((int)itemIdObj))
            {
                _logger.LogError("Could not find item with id : {ItemId}", itemIdObj);
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
