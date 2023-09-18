using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Content
    {
        public int ContentId { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        [Required]
        public string ContentText { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public int LessonId { get; set; }
        public virtual Lesson? Lesson { get; set; }
    }

    public enum ContentType
    {
        Text,
        Image,
        Video
    }
}
