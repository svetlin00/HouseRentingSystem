﻿using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Common.EntityValidations.Agent;
namespace HouseRentingSystem.Data.Models.Models
{
    public class Agent
    {
        public Agent()
        {
            Id = Guid.NewGuid();
            OwnedHouses = new HashSet<House>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public virtual ICollection<House> OwnedHouses { get; set; }
    }
}
