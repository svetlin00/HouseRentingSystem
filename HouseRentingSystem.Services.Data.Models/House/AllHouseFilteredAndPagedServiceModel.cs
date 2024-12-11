

using HouseRentingSystem.Web.ViewModels.House;

namespace HouseRentingSystem.Services.Data.Models.House
{
    public class AllHouseFilteredAndPagedServiceModel
    {
        public AllHouseFilteredAndPagedServiceModel()      
        {
            this.Houses = new HashSet<AllHousesViewModel>();
        }

        public int TotalHouseCount { get; set; }
        public IEnumerable<AllHousesViewModel> Houses { get; set; }
    }
}
