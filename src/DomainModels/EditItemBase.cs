using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModels
{
    class EditItemBase
    {
        [Required]
        public string Text { get; set; }
    }
}
