using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain;

namespace LMS.Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Administrator> Administrators { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=LENOVOLAPTOP;Database=LibraryManagementSystem;Integrated Security=True;Encrypt=False";
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasOne(m => m.Book)
                .WithOne(b => b.Borrower)
                .HasForeignKey<Member>(m => m.BorrowedBookId);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Borrower)
                .WithOne(m => m.Book)
                .HasForeignKey<Book>(b => b.BorrowerId);
        }
    }
}
