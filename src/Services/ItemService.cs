using DomainModels;
using Microsoft.EntityFrameworkCore;
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

            return new ItemDTO { Id = existingItem.Id, Text = existingItem.Text, CreatedBy = existingItem.CreatedBy, DateCreated = existingItem.DateCreated };
        }

        public IEnumerable<ItemDTO> GetAll()
        {
            return _itemRepository.All().Select(item => new ItemDTO { Id = item.Id, Text = item.Text, CreatedBy = item.CreatedBy, DateCreated = item.DateCreated });
        }

        public bool ItemExists(int id)
        {
            return _itemRepository.All().Any(item => item.Id == id);
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

        public ItemViewModel GetItemDetail(int id)
        {
            Item existingItem = _itemRepository.All().AsNoTracking().FirstOrDefault(item => item.Id == id);

            return new ItemViewModel { Id = existingItem.Id, Text = existingItem.Text, CreatedBy = existingItem.CreatedBy, DateCreated = existingItem.DateCreated };
        }

        public void AddItem(ItemDTO itemDto)
        {
            _itemRepository.Save(new Item { Text = itemDto.Text, CreatedBy = itemDto.CreatedBy, DateCreated = itemDto.DateCreated });
        }
        public void Update(ItemDTO itemDto)
        {
            _itemRepository.Save(new Item { Id = itemDto.Id, Text = itemDto.Text, CreatedBy = itemDto.CreatedBy, DateCreated = itemDto.DateCreated });
        }
        public void Delete(int id)
        {
            _itemRepository.Delete(id);
        }
    }
}