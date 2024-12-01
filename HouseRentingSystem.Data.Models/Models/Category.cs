
using System.ComponentModel.DataAnnotations;
using static HouseRentingSystem.Common.EntityValidations.Category;


namespace HouseRentingSystem.Data.Models.Models
{
    public class Category
    {
        public Category()
        {
            Houses = new HashSet<House>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLenght)]
        public string Name { get; set; } = null!;

        public virtual ICollection<House> Houses { get; set; }

    }
}
