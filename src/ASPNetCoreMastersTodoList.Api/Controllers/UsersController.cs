using ASPNetCoreMastersTodoList.Api.ApiModels;
using ASPNetCoreMastersTodoList.Api.BindingModels;
using DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using Services.DTO;
using System.Collections.Generic;
using System.Linq;

namespace ASPNetCoreMastersTodoList.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Authentication _authentication;

        public UsersController(IOptions<Authentication> options)
        {
            _authentication = options.Value;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return Ok(_authentication);
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Ok(_itemService.GetAll().Select(item => new ItemApiModel { Id = item.Id, Text = item.Text }));
        //}
    }
}