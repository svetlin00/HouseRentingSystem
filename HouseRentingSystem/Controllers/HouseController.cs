using HouseRentingSystem.Data.Models.Models;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static HouseRentingSystem.Common.NotificationMessagesConstants;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IAgentService agentService;
        private readonly IHouseService houseService;
        public HouseController(ICategoriesService categoriesService, IAgentService agentService, IHouseService houseService)
        {
            this.categoriesService = categoriesService;
            this.agentService = agentService;
            this.houseService = houseService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHouseQueryViewModel model)
        {
            AllHouseFilteredAndPagedServiceModel serviceModel = await houseService.AllAsync(model);
            model.Houses = serviceModel.Houses;
            model.TotalHouses = serviceModel.TotalHouseCount;
            model.Categories = await categoriesService.GetAllCategoryNameAsync();
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(this.User.GetId()!);

            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must be an agent";
                return this.RedirectToAction("Become", "Agent");
            }

            AddHouseFourmModel fourmModel = new AddHouseFourmModel()
            {
                SelectCategories = await categoriesService.GetAllCategorieAsync(),
            };
            return View(fourmModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddHouseFourmModel fourmModel)
        {
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(this.User.GetId()!);

            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must be an agent";
                return this.RedirectToAction("Become", "Agent");
            }

            bool existCategory = await this.categoriesService.ExistByIdAsync(fourmModel.CategoryId);
            if (!existCategory)
            {
                this.ModelState.AddModelError(nameof(fourmModel.CategoryId), "Selectet category does not exist");
            }
            if (!this.ModelState.IsValid)
            {
                fourmModel.SelectCategories = await categoriesService.GetAllCategorieAsync();
                return View(fourmModel);
            }

            try
            {
                string? agentId = await this.agentService.GetAgentIdAsync(this.User.GetId()!);
                await this.houseService.CreateAsync(fourmModel, agentId!);

            }
            catch (Exception)
            {

                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred");
                fourmModel.SelectCategories = await categoriesService.GetAllCategorieAsync();
                return this.View(fourmModel);
            }

            return this.RedirectToAction("All", "House");
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<AllHousesViewModel> myHouses = new List<AllHousesViewModel>();

            string userId = this.User.GetId()!;
            bool isUserAgent = await this.agentService.AgentExistByUserIdAsync(userId);
            if ((isUserAgent))
            {
                string? agentId = await this.agentService.GetAgentIdAsync(userId);
                myHouses.AddRange(await this.houseService.AllByAgentAsync(agentId!));
            }
            else
            {
                myHouses.AddRange(await this.houseService.AllByUserAsync(userId));
            }
            return View(myHouses);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id) 
        {
            bool exist = await this.houseService.ExistByIdAsync(id);
            if (!exist)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }

            HouseDetailsViewModel? viewModel = await this.houseService.GetDetailsByIdAsync(id);
            if (viewModel == null) 
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }
            else 
            {
                return View(viewModel);
            }
           
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            bool exist = await this.houseService.ExistByIdAsync(id);
            if (!exist)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(this.User.GetId()!);
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must be an agent";
                return this.RedirectToAction("Become", "Agent");
            }
            string? agentId = await this.agentService.GetAgentIdAsync(this.User.GetId()!);
            bool isAgentOwner = await this.houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(agentId!, id);
            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You are not the owner of the house!";
                return this.RedirectToAction("All", "House");
            }

            AddHouseFourmModel model = await this.houseService.GetHouseForEditByIdAsync(id);
            model.SelectCategories = await this.categoriesService.GetAllCategorieAsync();

          

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddHouseFourmModel model)
        {
            bool exist = await this.houseService.ExistByIdAsync(id);
            if (!exist)
            {
                this.TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(this.User.GetId()!);
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must be an agent";
                return this.RedirectToAction("Become", "Agent");
            }
            string? agentId = await this.agentService.GetAgentIdAsync(this.User.GetId()!);
            bool isAgentOwner = await this.houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(agentId!, id);
            if (!isAgentOwner)
            {
                this.TempData[ErrorMessage] = "You are not the owner of the house!";
                return this.RedirectToAction("All", "House");
            }

            bool existCategory = await this.categoriesService.ExistByIdAsync(model.CategoryId);
            if (!existCategory)
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Selectet category does not exist");
            }
            if (!this.ModelState.IsValid)
            {
                model.SelectCategories = await categoriesService.GetAllCategorieAsync();
                return View(model);
            }

            try
            {
                await this.houseService.EditAsync(id, model);
            }
            catch (Exception)
            {

                this.ModelState.AddModelError(string.Empty, "Unexpected error occurred");
                model.SelectCategories = await categoriesService.GetAllCategorieAsync();
                return this.View(model);
            }

            return this.RedirectToAction("All", "House");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            bool exist = await this.houseService.ExistByIdAsync(id);
            if (!exist)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }
            string? agentId = await this.agentService.GetAgentIdAsync(this.User.GetId()!);
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(this.User.GetId()!);
            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must be an agent";
                return this.RedirectToAction("Become", "Agent");
            }

            
            bool isOwner = await this.houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(agentId!, id);
            if (!isOwner)
            {
                TempData[ErrorMessage] = "You are not the owner of the house!";
                return this.RedirectToAction("All", "House");
            }

            DeleteHouseViewModel viewModel = await this.houseService.HouseToDeleteByHouseIdAsync(id);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, DeleteHouseViewModel model)
        {
            bool exist = await this.houseService.ExistByIdAsync(id);
            if (!exist)
            {
                TempData[ErrorMessage] = "House with the provided id does not exist!";
                return this.RedirectToAction("All", "House");
            }
            string? agentId = await this.agentService.GetAgentIdAsync(this.User.GetId()!);
            bool isOwner = await this.houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(agentId, id);
            if (!isOwner)
            {
                TempData[ErrorMessage] = "You are not the owner of the house!";
                return this.RedirectToAction("All", "House");
            }

            await  houseService.DeleteAsync(id);

            return RedirectToAction("All", "House");
        }

    }

    

}
