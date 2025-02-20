using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Data;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Data.Models.Models;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.ViewModels.House.Enums;
using HouseRentingSystem.Web.ViewModels.Agent;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

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
                HouseSorting.Newest => housesQuery.OrderByDescending(h => h.CreatedOn),
                HouseSorting.Oldest => housesQuery.OrderBy(h => h.CreatedOn),
                HouseSorting.PriceDescending => housesQuery.OrderByDescending(h => h.PricePerMonth),
                HouseSorting.PriceAscending => housesQuery.OrderBy(h => h.PricePerMonth),
                _ => housesQuery.OrderBy(h => h.RenterId != null)
                .ThenByDescending(h => h.CreatedOn)
            };
            IEnumerable<AllHousesViewModel> allHouses = await housesQuery
                .Where(h => h.IsActive == true)
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

        public async Task<IEnumerable<AllHousesViewModel>> AllByAgentAsync(string agentId)
        {
            IEnumerable<AllHousesViewModel> allAgentHouses = await this._context.Houses
                 .Where(h => h.AgentId.ToString() == agentId && h.IsActive == true)
                 .Select(h => new AllHousesViewModel
                 {
                     Id = h.Id.ToString(),
                     Title = h.Title,
                     Addres = h.Address,
                     ImageUrl = h.ImageUrl,
                     PricePerMonth = h.PricePerMonth,
                     IsRented = h.RenterId.HasValue
                 }).ToArrayAsync();
            return allAgentHouses;
        }

        public async Task<IEnumerable<AllHousesViewModel>> AllByUserAsync(string userId)
        {
            IEnumerable<AllHousesViewModel> allUserHoueses = await this._context.Houses
              .Where(h => h.RenterId.ToString() == userId && h.RenterId.HasValue && h.IsActive == true)
              .Select(h => new AllHousesViewModel
              {
                  Id = h.Id.ToString(),
                  Title = h.Title,
                  Addres = h.Address,
                  ImageUrl = h.ImageUrl,
                  PricePerMonth = h.PricePerMonth,
                  IsRented = h.RenterId.HasValue
              }).ToArrayAsync();
            return allUserHoueses;
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
                IsActive = true
            };
            await this._context.Houses.AddAsync(house);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> ExistByIdAsync(string id)
        {
            bool result = await this._context.Houses
                .Where(h => h.IsActive)
                .AnyAsync(h => h.Id.ToString() == id);

            return result;
        }

        public async Task<HouseDetailsViewModel?> GetDetailsByIdAsync(string houseId)
        {
            House? house = await this._context
                 .Houses
                 .Include(h => h.Category)
                 .Include(h => h.Agent)
                 .ThenInclude(a => a.User)
                 .Where(h => h.IsActive)
                 .FirstOrDefaultAsync(h => h.Id.ToString() == houseId);
            if (house == null)
            {
                return null;
            }
            else
            {
                return new HouseDetailsViewModel
                {
                    Id = house.Id.ToString(),
                    Title = house.Title,
                    Addres = house.Address,
                    ImageUrl = house.ImageUrl,
                    PricePerMonth = house.PricePerMonth,
                    IsRented = house.RenterId.HasValue,
                    Description = house.Description,
                    Category = house.Category.Name,
                    Agent = new AgentInfoOnHouseViewModel()
                    {
                        PhoneNumber = house.Agent.PhoneNumber,
                        Email = house.Agent.User.Email 
                    }
                };
            }
         
        }

        public async Task<AddHouseFourmModel> GetHouseForEditByIdAsync(string houseId)
        {
            House house = await this._context.Houses
                .Include(h => h.Category)
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return new AddHouseFourmModel
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = house.CategoryId
            };
        }

        public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
        {

            IEnumerable<IndexViewModel> lastThreeHouses = await this._context.Houses
                .Where(h => h.IsActive == true)
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
