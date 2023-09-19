﻿// <auto-generated />
using LMS.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LMS.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230919080002_CourseEnrollmentCascade")]
    partial class CourseEnrollmentCascade
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-rc.1.23419.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LMS.Models.Content", b =>
                {
                    b.Property<int>("ContentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContentId"));

                    b.Property<string>("ContentText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContentType")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContentId");

                    b.HasIndex("LessonId");

                    b.ToTable("Contents");

                    b.HasData(
                        new
                        {
                            ContentId = 1,
                            ContentText = "Sample text content for Lesson 1",
                            ContentType = 0,
                            LessonId = 1
                        },
                        new
                        {
                            ContentId = 2,
                            ContentText = "Sample text content for Lesson 2",
                            ContentType = 0,
                            LessonId = 2
                        });
                });

            modelBuilder.Entity("LMS.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EnrollmentCount")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("Courses");

                    b.HasData(
                        new
                        {
                            CourseId = 1,
                            Description = "Desc for sample course 1",
                            EnrollmentCount = 1,
                            Title = "Sample Course 1",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("LMS.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("Enrollments");

                    b.HasData(
                        new
                        {
                            EnrollmentId = 1,
                            CourseId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("LMS.Models.Lesson", b =>
                {
                    b.Property<int>("LessonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LessonId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("LessonId");

                    b.HasIndex("CourseId");

                    b.ToTable("Lessons");

                    b.HasData(
                        new
                        {
                            LessonId = 1,
                            CourseId = 1,
                            Description = "Description for Lesson 1",
                            Title = "Lesson 1"
                        },
                        new
                        {
                            LessonId = 2,
                            CourseId = 1,
                            Description = "Description for Lesson 2",
                            Title = "Lesson 2"
                        });
                });

            modelBuilder.Entity("LMS.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Password = "Test",
                            Role = 0,
                            UserName = "Test"
                        });
                });

            modelBuilder.Entity("LMS.Models.Content", b =>
                {
                    b.HasOne("LMS.Models.Lesson", "Lesson")
                        .WithMany("Contents")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lesson");
                });

            modelBuilder.Entity("LMS.Models.Course", b =>
                {
                    b.HasOne("LMS.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMS.Models.Enrollment", b =>
                {
                    b.HasOne("LMS.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("LMS.Models.User", "User")
                        .WithMany("Enrollments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMS.Models.Lesson", b =>
                {
                    b.HasOne("LMS.Models.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMS.Models.Course", b =>
                {
                    b.Navigation("Enrollments");

                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("LMS.Models.Lesson", b =>
                {
                    b.Navigation("Contents");
                });

            modelBuilder.Entity("LMS.Models.User", b =>
                {
                    b.Navigation("Enrollments");
                });
#pragma warning restore 612, 618
        }
    }
}
