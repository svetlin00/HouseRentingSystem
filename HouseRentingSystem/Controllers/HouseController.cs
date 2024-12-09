using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HouseRentingSystem.Common.NotificationMessagesConstants;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IAgentService agentService;
        public HouseController(ICategoriesService categoriesService, IAgentService agentService)
        {
            this.categoriesService = categoriesService;
            this.agentService = agentService;
        }

        public  async Task<IActionResult> All()
        {
            return View();
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
    }

}
