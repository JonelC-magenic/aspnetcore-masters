using ASPNetCoreMastersTodoList.Api.BindingModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels
{
    public class CreateItemCommand : ItemCreateBindingModel
    {
        public Item ToItem(ApplicationUser createdBy)
        {
            return new Item
            {
                Text = base.Text,
                CreatedBy = Guid.Parse(createdBy.Id)
            };
        }
    }
}
