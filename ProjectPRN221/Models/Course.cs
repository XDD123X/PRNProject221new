using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class Course
    {
        public Course()
        {
            EnroledCourses = new HashSet<EnroledCourse>();
            Explodes = new HashSet<Explode>();
            Quizzes = new HashSet<Quiz>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Title { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActived { get; set; }
        public long? EnrolNums { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<EnroledCourse> EnroledCourses { get; set; }
        public virtual ICollection<Explode> Explodes { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
    }
}
