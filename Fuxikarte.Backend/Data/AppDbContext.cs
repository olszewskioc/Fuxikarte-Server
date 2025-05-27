using Microsoft.EntityFrameworkCore;
using Fuxikarte.Backend.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fuxikarte.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Local> Locals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region USER

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<User>()
                .Property(u => u.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region CATEGORY

            modelBuilder.Entity<Category>()
                .Property(u => u.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Category>()
                .Property(u => u.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region CUSTOMER

            modelBuilder.Entity<Customer>()
                .Property(c => c.Phone)
                .HasColumnType("char(13)");

            modelBuilder.Entity<Customer>()
                .Property(c => c.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Customer>()
                .Property(c => c.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region LOCAL

            modelBuilder.Entity<Local>()
                .Property(l => l.Description)
                .HasColumnType("text");

            modelBuilder.Entity<Local>()
                .Property(l => l.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Local>()
                .Property(l => l.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region PRODUCT

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasColumnType("text");

            modelBuilder.Entity<Product>()
                .Property(p => p.Cost)
                .HasColumnType("money");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("money");

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Product>()
                .Property(p => p.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region SALE

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Local)
                .WithMany(l => l.Sales)
                .HasForeignKey(s => s.LocalId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Sale>()
                .Property(s => s.Payment)
                .HasConversion<string>();

            modelBuilder.Entity<Sale>()
                .Property(s => s.Subtotal)
                .HasColumnType("money");

            modelBuilder.Entity<Sale>()
                .Property(s => s.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Sale>()
                .Property(s => s.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion

            #region SALE_PRODUCT

            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Sale)
                .WithMany(s => s.SaleProducts)
                .HasForeignKey(sp => sp.SaleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SaleProducts)
                .HasForeignKey(sp => sp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SaleProduct>()
                .Property(sp => sp.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<SaleProduct>()
                .Property(sp => sp.UpdatedAt)
                .HasColumnType("timestamp with time zone")  // TODO: Create trigger or function before saveChanges() to set this value on update
                .HasDefaultValueSql("now()");

            #endregion
        }
    }
}