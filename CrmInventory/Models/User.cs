using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrmInventory.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string Profession { get; set; } = string.Empty;

        public virtual ICollection<MetExpense>? MetExpenses { get; set; }
    }
}