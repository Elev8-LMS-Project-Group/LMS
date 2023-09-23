using LMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<UserLessonProgress> UserLessonProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for testing and development
            
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLessonProgress>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserLessonProgresses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserLessonProgress>()
                .HasOne(e => e.Lesson)
                .WithMany(e => e.UserLessonProgresses)
                .HasForeignKey(e => e.LessonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "TestAdmin",
                    Password = "VnnDbCurBp5BSsZO15UwzA==;SywRr43i6tihyK+IHEAmdjsEbZZR96yRlW3DSod8j/A=", //Test
                    Role = UserRole.Admin
                } 
            );
        }
    }
}
