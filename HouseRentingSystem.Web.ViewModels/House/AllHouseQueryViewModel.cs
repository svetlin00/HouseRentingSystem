using HouseRentingSystem.Web.ViewModels.House.Enums;
using System.ComponentModel.DataAnnotations;
using static  HouseRentingSystem.Common.GeneralApplicationConstants;


namespace HouseRentingSystem.Web.ViewModels.House
{
    public class AllHouseQueryViewModel

       
    {
        public AllHouseQueryViewModel() 
        {
            this.HousesPerPage = EntitisPerPage;
            this.CurentPage = DefaultPage;

            this.Categories = new HashSet<string>();
            this.Houses = new HashSet<AllHousesViewModel>();
        }
        public string? Category { get; set; }

        [Display(Name ="Search House By")]
        public string? SearchString { get; set; }

        [Display(Name ="Sort House By")]
        public HouseSorting HouseSorting { get; set; }

        public int CurentPage { get; set; }

        public int HousesPerPage { get; set; }

        public int TotalHouses  { get; set; }

        public IEnumerable<string> Categories { get; set; } = null!;
        public IEnumerable<AllHousesViewModel> Houses { get; set; }

    }
}
