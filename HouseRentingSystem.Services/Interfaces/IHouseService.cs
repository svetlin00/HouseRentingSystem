using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;

namespace HouseRentingSystem.Services.Data.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();
        Task CreateAsync(AddHouseFourmModel model, string agenId);
        Task<AllHouseFilteredAndPagedServiceModel> AllAsync(AllHouseQueryViewModel model);
      
    }
}
