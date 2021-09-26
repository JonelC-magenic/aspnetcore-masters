using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO
{
    public class ItemByFilterDTO : Dictionary<string, string>
    {
        public ItemByFilterDTO(IDictionary<string, string> dictionaryCopy) : base(dictionaryCopy)
        {
        }
    }
}
