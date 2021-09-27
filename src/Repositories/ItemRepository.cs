﻿using DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _dataContext;

        public ItemRepository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public IQueryable<Item> All()
        {
            return _dataContext.Items.AsQueryable();
        }

        public void Save(Item item) {
            if (item.Id == default)
            {
                Item lastItem = _dataContext.Items.LastOrDefault();

                // Change this when we add the Key attribute to the item so that it autoincrements
                if (lastItem is null)
                    item.Id = 1;
                else
                    item.Id = lastItem.Id + 1;

                _dataContext.Items.Add(item);
            }
            else
            {
                Item itemToUpdate = _dataContext.Items.Find(existingItem => existingItem.Id == item.Id);

                if (itemToUpdate is null)
                    throw new Exception($"Item with id {item.Id} was not found.");

                itemToUpdate.Text = item.Text;
            }
        }

        public void Delete(int id)
        {
            Item itemToDelete = _dataContext.Items.Find(existingItem => existingItem.Id == id);

            if (itemToDelete is null)
                throw new Exception($"Item with id {id} was not found.");

            _dataContext.Items.Remove(itemToDelete);
        }
    }
}