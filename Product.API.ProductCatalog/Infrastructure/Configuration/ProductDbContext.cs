using Microsoft.EntityFrameworkCore;
using Product.API.ProductCatalog.Infrastructure.Entities;
namespace Product.API.ProductCatalog.Infrastructure.Configuration
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options):base(options)
        {
            
        }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ImageEntity> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageEntity>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId);

            modelBuilder.Entity<ProductEntity>()
                .Property(p=>p.Price)
                .HasColumnType("decimal(10:2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
