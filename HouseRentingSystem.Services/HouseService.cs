using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Data;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Data.Models.Models;

namespace HouseRentingSystem.Services.Data
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingSystemDbContext _context;

        public HouseService(HouseRentingSystemDbContext context) 
        {
            _context = context;
        }

        public async Task CreateAsync(AddHouseFourmModel model, string agentId)
        {
           House house = new House()
           {
               Title = model.Title,
               Address = model.Address,
               Description = model.Description,
               ImageUrl = model.ImageUrl,
               PricePerMonth = model.PricePerMonth,
               CategoryId = model.CategoryId,
               AgentId = Guid.Parse(agentId),
           };
            await this._context.Houses.AddAsync(house);
            await this._context.SaveChangesAsync();
        }

        public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
        {

            IEnumerable<IndexViewModel> lastThreeHouses = await this._context.Houses
                .OrderByDescending(h => h.CreatedOn)
                .Take(3)
                .Select(h => new IndexViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    ImageUrl = h.ImageUrl,

                }).ToArrayAsync();
                
           return lastThreeHouses;
        }

    }
}
