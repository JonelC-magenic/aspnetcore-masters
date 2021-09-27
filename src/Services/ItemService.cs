using DomainModels;
using Repositories;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

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
            Item existingItem = _itemRepository.All().FirstOrDefault(item => item.Id == id);

            if (existingItem is null)
                throw new Exception($"Item with id {id} was not found.");

            return new ItemDTO { Id = existingItem.Id, Text = existingItem.Text };
        }

        public IEnumerable<ItemDTO> GetAll()
        {
            return _itemRepository.All().Select(item => new ItemDTO { Id = item.Id, Text = item.Text });
        }

        public IEnumerable<ItemDTO> GetAllByFilter(ItemByFilterDTO filters)
        {
            var itemProperties = typeof(Item).GetProperties().Select(prop => prop.Name);
            var itemsToReturn = new List<Item>();

            foreach (var filter in filters)
            {
                if (string.Equals(filter.Key, "id", StringComparison.OrdinalIgnoreCase))
                {
                    itemsToReturn.AddRange(_itemRepository
                           .All()
                           .Where(item => item.Id == int.Parse(filter.Value)));
                }

                if (string.Equals(filter.Key, "text", StringComparison.OrdinalIgnoreCase))
                {
                    itemsToReturn.AddRange(_itemRepository
                        .All()
                        .Where(item => string.Equals(item.Text, filter.Value, StringComparison.InvariantCultureIgnoreCase)));
                }
            }

            return itemsToReturn.Distinct().Select(item => new ItemDTO { Id = item.Id, Text = item.Text });
        }

        public void AddItem(ItemDTO itemDto)
        {
            _itemRepository.Save(new Item { Text = itemDto.Text });
        }
        public void Update(ItemDTO itemDto)
        {
            _itemRepository.Save(new Item { Id = itemDto.Id, Text = itemDto.Text });
        }
        public void Delete(int id)
        {
            _itemRepository.Delete(id);
        }
    }
}