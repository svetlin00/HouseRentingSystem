using HouseRentingSystem.Web.ViewModels.Category;
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Common.EntityValidations.House;


namespace HouseRentingSystem.Web.ViewModels.House
{
    public class AddHouseFourmModel
    {
        public AddHouseFourmModel() 
        {
            this.SelectCategories = new HashSet<HouseSelectCategoryViewModel>();
        }
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(AddressMaxLength, MinimumLength = AddressMinLength)]
        public string Address { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(ImageUrlMaxLength)]
        [Display(Name ="Image Link")]
        public string ImageUrl { get; set; } = null!;


        [Range(typeof(decimal), PricePerMonthMinValue, PricePerMonthMaxValue)]
        [Display(Name ="Montly Price")]
        public decimal PricePerMonth { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [Display (Name ="Category")]
        public IEnumerable<HouseSelectCategoryViewModel> SelectCategories { get; set; }


    }
}
