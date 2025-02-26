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
        Task<IEnumerable<AllHousesViewModel>> AllByAgentAsync(string agentId);
        Task<IEnumerable<AllHousesViewModel>> AllByUserAsync(string userId);
        Task<HouseDetailsViewModel?> GetDetailsByIdAsync(string houseId);
        Task<bool> ExistByIdAsync(string id);
        Task<AddHouseFourmModel> GetHouseForEditByIdAsync(string houseId);
        Task<bool> IsAgentWithIdOwnerOfHouseWithIdAsync(string agentId, string houseId);
        Task EditAsync(string houseId, AddHouseFourmModel model);
        Task DeleteAsync(string houseId);
        Task<DeleteHouseViewModel> HouseToDeleteByHouseIdAsync(string houseId);
      
    }
}
