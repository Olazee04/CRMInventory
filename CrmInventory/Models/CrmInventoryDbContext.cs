using Microsoft.EntityFrameworkCore;

namespace CrmInventory.Models
{
    public class CrmInventoryDbContext : DbContext

    {
       public  DbSet<MetExpense> MetExpenses { get; set; }

        public CrmInventoryDbContext(DbContextOptions<CrmInventoryDbContext> options)
       : base(options)
        {

        }
    }

   
}
