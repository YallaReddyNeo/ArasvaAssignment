using Arasva.Core.Models;
using Arasva.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowingHistory> BorrowingHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(e => e.Author)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.HasMany<BorrowingHistory>()
                      .WithOne(b => b.Book)
                      .HasForeignKey(b => b.BookId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.HasMany<BorrowingHistory>()
                      .WithOne(b => b.Member)
                      .HasForeignKey(b => b.MemberId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BorrowingHistory>(entity =>
            {
                entity.ToTable("BorrowingHistory");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.BorrowFromDate)
                      .IsRequired();
            });
        }
    }
}
