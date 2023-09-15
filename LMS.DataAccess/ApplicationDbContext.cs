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

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "Test",
                    Password = "Test",
                    Role = UserRole.Admin
                } 
            );

            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    CourseId = 1,
                    Title = "Sample Course 1",
                    Description = "Desc for sample course 1",
                    UserId = 1,
                    EnrollmentCount = 1
                }    
            );

            modelBuilder.Entity<Lesson>().HasData(
                new Lesson
                {
                    LessonId = 1,
                    Title = "Lesson 1",
                    Description = "Description for Lesson 1",
                    CourseId = 1
                },
                new Lesson
                {
                    LessonId = 2,
                    Title = "Lesson 2",
                    Description = "Description for Lesson 2",
                    CourseId = 1
                }
            );

            modelBuilder.Entity<Content>().HasData(
                new Content
                {
                    ContentId = 1,
                    ContentType = ContentType.Text,
                    ContentText = "Sample text content for Lesson 1",
                    LessonId = 1
                },
                new Content
                {
                    ContentId = 2,
                    ContentType = ContentType.Text,
                    ContentText = "Sample text content for Lesson 2",
                    LessonId = 2
                }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment
                {
                    EnrollmentId = 1,
                    UserId = 1,
                    CourseId = 1
                }    
            );


        }
    }
}
