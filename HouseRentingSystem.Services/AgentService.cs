using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models.Models;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Agent;
using Microsoft.EntityFrameworkCore;


namespace HouseRentingSystem.Services.Data
{
    public class AgentService : IAgentService
    {
        private readonly HouseRentingSystemDbContext _dbContext;

        public AgentService(HouseRentingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  async Task<bool> AgentExistByPhoneNumberAsync(string phoneNumber)
        {
            bool exist = await this._dbContext
            .Agents
                .AnyAsync(a => a.PhoneNumber == phoneNumber);
         return exist;
         
        }

        public async Task<bool> AgentExistByUserIdAsync(string userId)
        {
            bool result = await this._dbContext
                .Agents
                .AnyAsync(a => a.UserId.ToString() == userId);
            return result;
        }

  

        public  async Task CreateAsync(string userId, BecomeFourmModel model)
        {
            Agent agent = new Agent()
            {
                PhoneNumber = model.PhoneNumber,
                UserId = Guid.Parse(userId)

            };
            await this._dbContext.AddAsync(agent);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<string?> GetAgentIdAsync(string userId)
        {
            Agent? agent = await this._dbContext.Agents.FirstOrDefaultAsync(a => a.UserId.ToString() == userId);
            if (agent == null) 
            {
                return null;
            }
            return agent.Id.ToString();
        }

        public async Task<bool> UserHasRentsByUserIdAsync(string userId)
        {
            ApplicationUser? user = await this._dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null) 
            {
                return false;
            }

            return user.RentedHouses.Any();
        }
    }
}
 