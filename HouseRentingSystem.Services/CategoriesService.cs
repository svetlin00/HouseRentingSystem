

using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly HouseRentingSystemDbContext dbContext; 
        public CategoriesService(HouseRentingSystemDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<HouseSelectCategoryViewModel>> GetAllCategorieAsync()
        {
            IEnumerable<HouseSelectCategoryViewModel> categories = await dbContext.Categories
                .Select(c => new HouseSelectCategoryViewModel 
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToArrayAsync();
            return categories;
        }


        public async Task<bool> ExistByIdAsync(int id)
        {
            bool result = await this.dbContext.Categories
                .AnyAsync(c => c.Id == id);
            return result;
        }
    }
}
 