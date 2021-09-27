using DomainModels;
using System.Linq;

namespace Repositories
{
    public interface IItemRepository
    {
        IQueryable<Item> All();
        void Delete(int id);
        void Save(Item item);
    }
}