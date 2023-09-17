using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        [Required]
        [StringLength(100)]
        public String Title { get; set; }
        [StringLength(500)]
        public String Description { get; set; }
        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }
        public virtual ICollection<Content>? Contents { get; set; }
    }
}
