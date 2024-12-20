﻿using HouseRentingSystem.Data.Models.Models;
using HouseRentingSystem.Web.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Services.Data.Interfaces
{
    public interface  ICategoriesService
    {
        Task<IEnumerable<HouseSelectCategoryViewModel>> GetAllCategorieAsync();
        Task<bool> ExistByIdAsync(int id);
        Task<IEnumerable<string>> GetAllCategoryNameAsync();
    }
}
