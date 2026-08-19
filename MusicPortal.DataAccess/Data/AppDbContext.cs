using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Models;
using System.Collections.Generic;

namespace MusicPortal.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Song> Songs { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Username).IsRequired().HasMaxLength(100);
                b.Property(u => u.Email).IsRequired().HasMaxLength(150);
                b.Property(u => u.PasswordHash).IsRequired();
                b.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder.Entity<Role>()
                .Property(r => r.Name).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Name).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Artist>(b =>
            {
                b.Property(a => a.Name).IsRequired().HasMaxLength(100);
                b.Property(a => a.Bio).HasMaxLength(1000);
            });

            modelBuilder.Entity<Artist>()
                .HasOne(a => a.User)
                .WithOne(u => u.ArtistProfile)
                .HasForeignKey<Artist>("UserId")
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<Song>(b =>
            {
                b.Property(s => s.Title).IsRequired().HasMaxLength(150);
                b.Property(s => s.FilePath).IsRequired();

                b.HasOne(s => s.Artist)
                    .WithMany(a => a.Songs)
                    .HasForeignKey(s => s.ArtistId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Property(s => s.Rating)
                    .HasColumnType("float")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<Song>()
                .HasMany(s => s.Genres)
                .WithMany(g => g.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongGenre",
                    j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Song>().WithMany().HasForeignKey("SongId").OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder.Entity<Comment>(b =>
            {
                b.Property(c => c.Text).IsRequired().HasMaxLength(1000);

                b.HasOne(c => c.Song).WithMany(s => s.Comments)
                    .HasForeignKey("SongId").OnDelete(DeleteBehavior.Cascade);

                b.HasOne(c => c.User).WithMany(u => u.Comments)
                    .HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Rating>(b =>
            {
                b.HasIndex("UserId", "SongId").IsUnique();

                b.HasOne(r => r.Song).WithMany(s => s.Ratings)
                    .HasForeignKey("SongId").OnDelete(DeleteBehavior.Cascade);

                b.HasOne(r => r.User).WithMany(u => u.Ratings)
                    .HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" },
                new Role { Id = 3, Name = "Artist" },
                new Role { Id = 4, Name = "Pending" }
            );
        }
    }
}