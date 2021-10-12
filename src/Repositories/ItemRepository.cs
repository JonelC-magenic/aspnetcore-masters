using DomainModels;
using System;
using System.Linq;

namespace Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ItemDbContext _dataContext;

        public ItemRepository(ItemDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public IQueryable<Item> All()
        {
            return _dataContext.Items.AsQueryable();
        }

        public void Save(Item item)
        {
            if (item.Id == default)
            {
                _dataContext.Items.Add(item);
            }
            else
            {
                _dataContext.Items.Update(item);
            }

            _dataContext.SaveChanges();
        }

        public void Delete(int id)
        {
            Item itemToDelete = _dataContext.Items.Find(id);

            _dataContext.Items.Remove(itemToDelete);

            _dataContext.SaveChanges();
        }
    }
}
