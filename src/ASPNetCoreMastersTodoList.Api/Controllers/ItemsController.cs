using ASPNetCoreMastersTodoList.Api.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;
using System.Collections.Generic;

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
        public IActionResult GetAll([FromQuery] Dictionary<string, string> filters)
        {
            if (!(filters is null)) // change to filters is not null when upgraded to c# 9 or greater
            {
                return GetByFilters(filters);
            }

            return Ok(_itemService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {

            return Ok(_itemService.Get(id));
        }

        public IActionResult GetByFilters([FromQuery]Dictionary<string, string> filters)
        {
            var filterDto = new ItemByFilterDTO(filters);

            return Ok(_itemService.GetAllByFilter(filterDto));
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ItemUpdateBindingModel itemUpdateBindingModel)
        {
            if (ModelState.IsValid)
            {
                _itemService.Update(new ItemDTO
                {
                    Text = itemUpdateBindingModel.Text
                });
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _itemService.Delete(id);
            return Ok();
        }
    }
}