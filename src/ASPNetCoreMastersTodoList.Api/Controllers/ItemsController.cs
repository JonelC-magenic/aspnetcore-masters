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

        public void Post(ItemCreateBindingModel itemCreateBindingModel)
        {
            new ItemService().Save(new ItemDTO
            {
                Text = itemCreateBindingModel.Text
            });
        }
    }
}