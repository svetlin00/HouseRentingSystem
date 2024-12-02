using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HouseRentingSystem.Data.Models.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() 
        {
            this.RentedHouses = new HashSet<House>();
        }

        public ICollection<House> RentedHouses { get; set; }
    }

    
}