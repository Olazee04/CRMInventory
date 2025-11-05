using System.ComponentModel.DataAnnotations.Schema;
  
namespace CrmInventory.Models
{
    public class MetExpense
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public decimal Value { get; set; }

        // 👇 Relationship: each Expense belongs to ONE User
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }

}

