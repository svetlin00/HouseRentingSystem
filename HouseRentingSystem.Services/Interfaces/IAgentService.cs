using HouseRentingSystem.Web.ViewModels.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Services.Data.Interfaces
{
    public interface IAgentService
    {
        Task<bool> AgentExistByUserIdAsync(string userId);
        Task<bool> AgentExistByPhoneNumberAsync(string phoneNumber);
        Task<bool> UserHasRentsByUserIdAsync(string userId);
        Task CreateAsync(string userId, BecomeFourmModel model);
        Task<string?> GetAgentIdAsync(string userId);
    }
}
