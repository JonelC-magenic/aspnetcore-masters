using DomainModels;
using Services.DTO;
using System.Collections.Generic;

namespace Services
{
    public interface IItemService
    {
        void AddItem(ItemDTO itemDto);
        void Delete(int id);
        ItemDTO Get(int id);
        ItemViewModel GetItemDetail(int id);
        bool ItemExists(int id);
        IEnumerable<ItemDTO> GetAll();
        IEnumerable<ItemDTO> GetAllByFilter(ItemByFilterDTO filters);
        void Update(ItemDTO itemDto);
    }
}