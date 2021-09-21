using ASPNetCoreMastersTodoList.Api.BindingModels;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

namespace ASPNetCoreMastersTodoList.Api.Controllers
{
    public class ItemsController : ControllerBase
    {
        public int Get(int id)
        {
            return new ItemService().Get(id);
        }

        public void Post([FromBody]ItemCreateBindingModel itemCreateBindingModel)
        {
            if (ModelState.IsValid)
            {
                new ItemService().Save(new ItemDTO
                {
                    Text = itemCreateBindingModel.Text
                });
            }
        }
    }
}