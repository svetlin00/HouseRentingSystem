using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Web.ViewModels.House
{
    public class AllHousesViewModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Addres { get; set; } = null!;
        [Display(Name ="Image Link")]
        public string ImageUrl { get; set; } = null!;
        [Display(Name ="Price er month")]
        public decimal PricePerMonth { get; set; }

        [Display(Name ="Is Rented")]
        public bool IsRented { get; set; }


    }
}
