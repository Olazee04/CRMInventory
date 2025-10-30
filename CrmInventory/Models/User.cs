using System.ComponentModel.DataAnnotations;

namespace CrmInventory.Models
{
    public class User
    {
        [Key]  // Primary Key
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Profession { get; set; }
    }
}
