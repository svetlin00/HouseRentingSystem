using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Data
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingSystemDbContext _context;

        public HouseService(HouseRentingSystemDbContext context) 
        {
            _context = context;
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
