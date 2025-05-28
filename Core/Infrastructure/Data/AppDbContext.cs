using InventoryControl.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do Item
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Name).IsRequired().HasMaxLength(100);
                entity.Property(i => i.SKU).IsRequired();
                entity.HasIndex(i => i.SKU).IsUnique();
                entity.Property(i => i.Quantity).IsRequired();
                entity.Property(i => i.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                
                entity.HasOne(i => i.Supplier)
                    .WithMany(s => s.Items)
                    .HasForeignKey(i => i.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração do Supplier
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.CNPJ).IsRequired();
                entity.HasIndex(s => s.CNPJ).IsUnique();
                entity.Property(s => s.Phone).HasMaxLength(20);
            });

            // Configuração do User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
                entity.Property(u => u.CreatedAt).IsRequired();
            });
        }
    }
}