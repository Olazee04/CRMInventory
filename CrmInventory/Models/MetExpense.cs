using System.ComponentModel.DataAnnotations;
  
namespace CrmInventory.Models
{
    public class MetExpense
    {
        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public decimal Value { get; set; }
    }

}

