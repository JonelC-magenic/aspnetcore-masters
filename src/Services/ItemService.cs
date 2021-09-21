using System;
using System.Collections.Generic;

namespace Services
{
    public class ItemService
    {
        public IEnumerable<string> GetAll(int id)
        {
            return new List<string>() { "item 1" };
        }
    }
}
