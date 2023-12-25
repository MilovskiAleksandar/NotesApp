using Microsoft.EntityFrameworkCore;
using SEDC.NotesAppFluentApi.Domain.Enums;
using SEDC.NotesAppFluentApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEDC.NotesAppFluentApi.DataAccess
{
    public class NotesAppDbContext : DbContext
    {
        public NotesAppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //NOTE MAPPING
            modelBuilder.Entity<Note>()
                .Property(x => x.Text)
                .HasMaxLength(50)
                .IsRequired(); 

            modelBuilder.Entity<Note>()
            .Property(x => x.Tag) 
            .IsRequired(); 

            modelBuilder.Entity<Note>()
            .Property(x => x.Priority) 
            .IsRequired(); 

            //RELATION
            modelBuilder.Entity<Note>()
                .HasOne(x => x.User)
                .WithMany(x => x.Notes)
                .HasForeignKey(x => x.UserId);

            //USER
            modelBuilder.Entity<User>()
               .Property(x => x.FirstName) 
               .HasMaxLength(50); 

            modelBuilder.Entity<User>()
            .Property(x => x.LastName) 
            .HasMaxLength(50); 

            modelBuilder.Entity<User>()
            .Property(x => x.UserName) 
            .HasMaxLength(20) 
            .IsRequired();

            modelBuilder.Entity<User>()
                .Ignore(x => x.Age);

            modelBuilder.Entity<User>()
                .Property(x => x.Password)
                .IsRequired()
                .HasDefaultValue("test");
            
           

            //SEED...

            modelBuilder.Entity<User>()
                            .HasData(
                                new User { Id = 1, FirstName = "John", LastName = "Doe", UserName = "john_doe" },
                                new User { Id = 2, FirstName = "Jane", LastName = "Smith", UserName = "jane_smith" }
                            );
            modelBuilder.Entity<Note>()
                .HasData(
                    new Note { Id = 1, Text = "Buy groceries", Priority = Priority.High, Tag = Tag.SocialLife, UserId = 1 },
                    new Note { Id = 2, Text = "Finish project report", Priority = Priority.Medium, Tag = Tag.Work, UserId = 2 },
                    new Note { Id = 3, Text = "Call friends", Priority = Priority.Low, Tag = Tag.Health, UserId = 1 }
                );
        }
    }
}
