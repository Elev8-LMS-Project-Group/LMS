using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Category { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; } //Instructor
        public int EnrollmentCount { get; set; }
        // Navigation to represent the lessons in this course
        public virtual ICollection<Lesson>? Lessons { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}
