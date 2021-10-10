using ASPNetCoreMastersTodoList.Api.ApiModels;
using ASPNetCoreMastersTodoList.Api.BindingModels;
using ASPNetCoreMastersTodoList.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Collections.Generic;
using System.Linq;

namespace ASPNetCoreMastersTodoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService ?? throw new System.ArgumentNullException(nameof(itemService));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_itemService.GetAll().Select(item => new ItemApiModel { Id = item.Id, Text = item.Text }));
        }

        [HttpGet("{id}"), EnsureItemExistsAttribute]
        public IActionResult Get(int id)
        {
            ItemDTO existingItem = _itemService.Get(id);

            return Ok(new ItemApiModel { Id = existingItem.Id, Text = existingItem.Text });
        }

        [HttpGet("filterBy")]
        public IActionResult GetByFilters([FromQuery]Dictionary<string, string> filters)
        {
            var filterDto = new ItemByFilterDTO(filters);

            return Ok(_itemService.GetAllByFilter(filterDto).Select(item => new ItemApiModel { Id = item.Id, Text = item.Text }));
        }

        [HttpPost]
        public IActionResult Post([FromBody]ItemCreateBindingModel itemCreateBindingModel)
        {
            if (ModelState.IsValid)
            {
                _itemService.AddItem(new ItemDTO
                {
                    Text = itemCreateBindingModel.Text
                });
            }

            return Ok();
        }

        [HttpPut("{id}"), EnsureItemExistsAttribute]
        public IActionResult Put(int id, [FromBody]ItemUpdateBindingModel itemUpdateBindingModel)
        {
            if (ModelState.IsValid)
            {
                _itemService.Update(new ItemDTO
                {
                    Id = id,
                    Text = itemUpdateBindingModel.Text
                });
            }
            return Ok();
        }

        [HttpDelete("{id}"), EnsureItemExistsAttribute]
        public IActionResult Delete(int id)
        {
            _itemService.Delete(id);
            return Ok();
        }
    }
}