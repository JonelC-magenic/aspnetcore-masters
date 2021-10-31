using ASPNetCoreMastersTodoList.Api.CustomAttribute;
using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreMastersTodoList.Api.BindingModels
{
    public class ItemCreateBindingModel
    {
        [Required]
        [StringLength(128, MinimumLength = 1)]
        [CheckNumberWordsAttribute(3)]
        public string Text { get; set; }
    }
}