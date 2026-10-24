using BulletinBoard.DAL.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.DAL.EF
{
    /// <summary>Контекст EF Core (Code First): структура БД будується з класів-сутностей.</summary>
    public class BoardContext : DbContext
    {
        public BoardContext(DbContextOptions<BoardContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Ad> Ads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.Property(u => u.Name).IsRequired().HasMaxLength(100);
                user.Property(u => u.Email).IsRequired().HasMaxLength(200);
                user.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Category>(category =>
            {
                category.Property(c => c.Name).IsRequired().HasMaxLength(100);
                category.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey(c => c.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Tag>(tag =>
            {
                tag.Property(t => t.Name).IsRequired().HasMaxLength(50);
                tag.HasIndex(t => t.Name).IsUnique();
            });

            modelBuilder.Entity<Ad>(ad =>
            {
                ad.Property(a => a.Title).IsRequired().HasMaxLength(150);
                ad.Property(a => a.Description).HasMaxLength(2000);
                ad.HasOne(a => a.Author)
                    .WithMany()
                    .HasForeignKey(a => a.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
                ad.HasOne(a => a.Category)
                    .WithMany()
                    .HasForeignKey(a => a.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                ad.HasMany(a => a.Tags)
                    .WithMany()
                    .UsingEntity(join => join.ToTable("AdTags"));
            });
        }
    }
}
