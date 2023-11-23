using Microsoft.EntityFrameworkCore;
using Student_Platform_DB_Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_DB_Exam
{
    internal class UniContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Worker> Workers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=University_EXAM;Trusted_Connection=True;");

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)//sitas metodas optional
        //{
        //    //base.OnModelCreating(modelBuilder); - galima istrinti

        //    //configure many2many
        //    modelBuilder.Entity<BookCategory>().//turi turet du raktus
        //        HasKey(bc => new { bc.BookId, bc.CategoryId });//NURODO PRIMARY KEY is 2 stulpeliu; TODEL NEREIKIA ATSKIRAI NURODYTI [KEY] KLASEJE
        //    //author klasej jau nusirodem

        //    modelBuilder.Entity<BookCategory>()//uztikrinam join, sakom kokiu budu per koki rakta jungias i kokia lentele
        //        .HasOne(bc => bc.Book)//book category jungiasi su book, viena knyga
        //        .WithMany(b => b.BookCategories)
        //        .HasForeignKey(b => b.BookId).OnDelete(DeleteBehavior.Cascade);//per rakta bookid

        //    modelBuilder.Entity<BookCategory>()//si lentele jungiasi su category bet rakta categoryid
        //        .HasOne(bc => bc.Category)
        //        .WithMany(b => b.BookCategories)
        //        .HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.Restrict);

        //}

    }
}
