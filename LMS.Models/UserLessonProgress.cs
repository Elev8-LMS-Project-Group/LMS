using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class UserLessonProgress
    {
        [Key]
        public int UserLessonProgressId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; }
        public bool IsCompleted { get; set; }
    }
}
