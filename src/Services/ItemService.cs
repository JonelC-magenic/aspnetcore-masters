using DomainModels;
using Repositories;
using Services.DTO;
using System.Collections.Generic;

namespace Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository ?? throw new System.ArgumentNullException(nameof(itemRepository));
        }

        public ItemDTO Get(int id)
        {
            return new ItemDTO { Text = "This is a sample text" };
        }

        public IEnumerable<ItemDTO> GetAll()
        {
            return new List<ItemDTO>() { new ItemDTO { Text = "Sanple Text yessir" } };
        }

        public IEnumerable<ItemDTO> GetAllByFilter(ItemByFilterDTO filters)
        {
            return new List<ItemDTO>() { new ItemDTO { Text = "Sanple Text yessir" } };
        }

        public void AddItem(ItemDTO itemDto)
        {

        }
        public void Update(ItemDTO itemDto)
        {

        }
        public void Delete(int id)
        {
        }

        public void Save(ItemDTO itemDTO)
        {
            var item = new Item { Text = itemDTO.Text };
        }
    }
}