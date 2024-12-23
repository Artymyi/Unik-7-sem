using Microsoft.EntityFrameworkCore;
using PopkovFinance.Models;

namespace PopkovFinance.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Сущности
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Модель для пользователей
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Модель для категорий
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Модель для транзакций
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Amount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(t => t.Date)
                      .IsRequired();

                entity.Property(t => t.IsIncome)
                      .IsRequired();

                entity.HasOne(t => t.Category)
                      .WithMany()
                      .HasForeignKey(t => t.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.User)
                      .WithMany()
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Модель для целей
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(g => g.TargetAmount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(g => g.CurrentAmount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)")
                      .HasDefaultValue(0);

                entity.Property(g => g.DueDate)
                      .IsRequired();
            });
        }
    }
}

