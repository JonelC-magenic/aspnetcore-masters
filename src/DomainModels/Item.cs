using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }
}