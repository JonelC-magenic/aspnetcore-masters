using ASPNetCoreMastersTodoList.Api.ApiModels;
using ASPNetCoreMastersTodoList.Api.BindingModels;
using ASPNetCoreMastersTodoList.Api.Filters;
using DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]"), EnsureItemExists]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IAuthorizationService _authService;
        private readonly UserManager<ApplicationUser> _userService;
        private readonly ILogger _logger;

        public ItemsController(
            IItemService itemService,
            IAuthorizationService authService,
            UserManager<ApplicationUser> userService, 
            ILogger<ItemsController> logger)
        {
            _itemService = itemService;
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<ItemApiModel> items = _itemService
                .GetAll()
                .Select(item => new ItemApiModel { Id = item.Id, Text = item.Text, CreatedBy = item.CreatedBy, DateCreated = item.DateCreated });

            _logger.LogInformation("Loaded {ItemCount} items", items.Count());

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation("Loading item with id {ItemId}", id);

            ItemDTO existingItem = _itemService.Get(id);

            return Ok(new ItemApiModel { Id = existingItem.Id, Text = existingItem.Text, CreatedBy = existingItem.CreatedBy, DateCreated = existingItem.DateCreated });
        }

        [HttpGet("filterBy")]
        public IActionResult GetByFilters([FromQuery]Dictionary<string, string> filters)
        {
            var filterDto = new ItemByFilterDTO(filters);

            return Ok(_itemService.GetAllByFilter(filterDto).Select(item => new ItemApiModel { Id = item.Id, Text = item.Text }));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ItemCreateBindingModel itemCreateBindingModel)
        {
            if (ModelState.IsValid)
            {
                var email = ((ClaimsIdentity)User.Identity).FindFirst("Email");
                var user = await _userService.FindByNameAsync(email.Value);
                _itemService.AddItem(new ItemDTO
                {
                    Text = itemCreateBindingModel.Text,
                    CreatedBy = Guid.Parse(user.Id),
                    DateCreated = DateTime.UtcNow
                });

                _logger.LogInformation("Item has been created");
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ItemUpdateBindingModel itemUpdateBindingModel)
        {
            if (ModelState.IsValid)
            {
                ItemViewModel item = _itemService.GetItemDetail(id);
                var authResult = await _authService.AuthorizeAsync(User, new Item { CreatedBy = item.CreatedBy }, "OnlyCreatorCanEditItem");
                if (!authResult.Succeeded)
                    return new ForbidResult();

                _itemService.Update(new ItemDTO
                {
                    Id = id,
                    Text = itemUpdateBindingModel.Text,
                    CreatedBy = item.CreatedBy,
                    DateCreated = item.DateCreated
                });

                _logger.LogInformation("Successfully updated item with id : {ItemId}", id);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _itemService.Delete(id);

            _logger.LogInformation("Successfully deleted item with id : {ItemId}", id);

            return Ok();
        }
    }
}