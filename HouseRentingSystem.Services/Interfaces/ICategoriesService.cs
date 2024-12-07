using HouseRentingSystem.Data.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Services.Data.Interfaces
{
    public interface  ICategoriesService
    {
        public Task<ICollection<Category>> GetAllCategorieAsync();
    }
}
