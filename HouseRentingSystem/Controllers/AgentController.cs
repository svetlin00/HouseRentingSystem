﻿using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.Agent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HouseRentingSystem.Common.NotificationMessagesConstants;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;
        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }
        [HttpGet]
        public async Task<IActionResult> Become()
        {
            string? userId = this.User.GetId();
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(userId);
            if (isAgent)
            {
                this.TempData[ErrorMessage] = "You are already an agent!";
                return this.RedirectToAction("Index", "Home");
            }
            return this.View();
        }
        [HttpPost]
        public async Task<IActionResult> Become(BecomeFourmModel model) 
        {
            string? userId = this.User.GetId();
            bool isAgent = await this.agentService.AgentExistByUserIdAsync(userId);
            if (isAgent)
            {
                this.TempData[ErrorMessage] = "You are already an agent!";
                return this.RedirectToAction("Index", "Home");
            }

            bool isPhoneNumberTake = await this.agentService.AgentExistByPhoneNumberAsync(model.PhoneNumber);
            if (isPhoneNumberTake)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Agent with the provided phone number alrady exist!");
            }
            if(!this.ModelState.IsValid)
            {
                return this.View(model);

            }

            bool userHasActiveRents = await this.agentService.UserHasRentsByUserIdAsync(userId);

            if (userHasActiveRents) 
            {
                this.TempData[ErrorMessage] = "You must not have any active rents in order to becom an agent!";
                return RedirectToAction("Mine", "House");
            }
            try
            {
                await this.agentService.CreateAsync(userId, model);
            }
            catch (Exception e)
            {

                this.TempData[ErrorMessage] = "Unexpected error occurred while registering you as an agent!";
                return this.RedirectToAction("Index", "Home");
            }
            return this.RedirectToAction("All", "House");
           
        }
    }
}
