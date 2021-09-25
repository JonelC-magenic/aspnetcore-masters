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
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new ItemService().GetAll(1));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(new ItemService().Get(id));
        }

        public IActionResult GetByFilters([FromQuery]Dictionary<string, string> filters)
        {
            return Ok(new ItemService().Get(1));
        }

        [HttpPost]
        public IActionResult Post([FromBody]ItemCreateBindingModel itemCreateBindingModel)
        {
            if (ModelState.IsValid)
            {
                new ItemService().Save(new ItemDTO
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
                new ItemService().Save(new ItemDTO
                {
                    Text = itemUpdateBindingModel.Text
                });
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(new ItemService().Get(id));
        }
    }
}