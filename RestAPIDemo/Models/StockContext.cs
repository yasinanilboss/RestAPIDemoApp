using Microsoft.EntityFrameworkCore;

namespace RestAPIDemo.Models
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
               
        }

        public DbSet<Stock> Stocks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Stock>()
                .Property(p => p.KDV)
                .HasColumnType("decimal(18,4)");
        }
    }
}
