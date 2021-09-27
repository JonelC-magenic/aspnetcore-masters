using Services.DTO;
using System.Collections.Generic;

namespace Services
{
    public interface IItemService
    {
        void AddItem(ItemDTO itemDto);
        void Delete(int id);
        ItemDTO Get(int id);
        IEnumerable<ItemDTO> GetAll();
        IEnumerable<ItemDTO> GetAllByFilter(ItemByFilterDTO filters);
        void Update(ItemDTO itemDto);
    }
}