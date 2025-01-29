using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions){

        }
        // DbSets for the entities
       // DbSets for the entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            //Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();    

            //User - role
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

        // Dummy Roles Data
             modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Personel" },
                new Role { Id = 3, Name = "Hemsire" }
            );

            // Dummy Users Data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("b8a3e9e1-b7c9-4d8e-9f1e-91e3f741dcad"),
                    Name = "Mustafa",
                    Surname = "ÇOBAN",
                    Email = "mustafa@example.com",
                    Phone = "123456789",
                    Password = "hashedPassword"  // Password should be hashed before saving
                },
                new User
                {
                    Id = Guid.Parse("39e1fa4d-75f2-4e1b-b2d3-bf54bb0d785b"),
                    Name = "Lale",
                    Surname = "Bitki",
                    Email = "bitki@example.com",
                    Phone = "987654321",
                    Password = "hashedPassword"  // Password should be hashed before saving
                }
            );
            // Seed UserRole Data to associate users with roles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = Guid.Parse("b8a3e9e1-b7c9-4d8e-9f1e-91e3f741dcad"), RoleId = 1 },
                new UserRole { UserId = Guid.Parse("b8a3e9e1-b7c9-4d8e-9f1e-91e3f741dcad"), RoleId = 2 },
                new UserRole { UserId = Guid.Parse("39e1fa4d-75f2-4e1b-b2d3-bf54bb0d785b"), RoleId = 3 }
            );

        }
    }
}