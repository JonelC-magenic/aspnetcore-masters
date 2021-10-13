using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreMastersTodoList.Api.BindingModels
{
    public class ConfirmBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}