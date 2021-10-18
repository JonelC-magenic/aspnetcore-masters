using System;
using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}