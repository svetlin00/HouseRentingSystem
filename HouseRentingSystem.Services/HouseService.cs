using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Data;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Data.Models.Models;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.ViewModels.House.Enums;

namespace HouseRentingSystem.Services.Data
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingSystemDbContext _context;

        public HouseService(HouseRentingSystemDbContext context) 
        {
            _context = context;
        }

        public async Task<AllHouseFilteredAndPagedServiceModel> AllAsync(AllHouseQueryViewModel model)
        {
            IQueryable<House> housesQuery = this._context.Houses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.Category))
            {
                housesQuery = housesQuery.Where(h => h.Category.Name == model.Category);
            }
            if (!string.IsNullOrWhiteSpace(model.SearchString))
            {
                string wildCard = $"%{model.SearchString.ToLower()}%";

                housesQuery = housesQuery.Where(h =>
                EF.Functions.Like(h.Title, wildCard) ||
                EF.Functions.Like(h.Address, wildCard) ||
                EF.Functions.Like(h.Description, wildCard));
            }

            housesQuery = model.HouseSorting switch
            {
                HouseSorting.Newest => housesQuery.OrderBy(h => h.CreatedOn),
                HouseSorting.Oldest => housesQuery.OrderByDescending(h => h.CreatedOn),
                HouseSorting.PriceDescending => housesQuery.OrderByDescending(h => h.PricePerMonth),
                HouseSorting.PriceAscending => housesQuery.OrderBy(h => h.PricePerMonth),
                _ => housesQuery.OrderBy(h => h.RenterId != null)
                .ThenByDescending(h => h.CreatedOn)
            };
            IEnumerable<AllHousesViewModel> allHouses = await housesQuery
                .Skip((model.CurentPage - 1) * model.HousesPerPage)
                .Take(model.HousesPerPage)
                .Select(h => new AllHousesViewModel()
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Addres = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue


                }).ToArrayAsync();

            int totalHouses = housesQuery.Count();

            return new AllHouseFilteredAndPagedServiceModel()
            {
                Houses = allHouses,
                TotalHouseCount = totalHouses
            };
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
               CreatedOn = DateTime.Now,
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
