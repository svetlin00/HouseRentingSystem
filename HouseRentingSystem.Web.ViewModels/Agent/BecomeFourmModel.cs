
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Common.EntityValidations.Agent;

namespace HouseRentingSystem.Web.ViewModels.Agent
{
    public class BecomeFourmModel
    {
        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Phone]
        [Display(Name ="Phone")]
        public string PhoneNumber { get; set; }
    }
}
