using DomainModels;
using Services.DTO;
using System.Collections.Generic;

namespace Services
{
    public class ItemService
    {
        public int Get(int id)
        {
            return id;
        }

        public IEnumerable<string> GetAll(int id)
        {
            return new List<string>() { "item 1" };
        }

        public void Save(ItemDTO itemDTO)
        {
            var item = new Item { Text = itemDTO.Text };
        }
    }
}