using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
            EnroledCourses = new HashSet<EnroledCourse>();
            HistoryQuizzes = new HashSet<HistoryQuiz>();
            Tokens = new HashSet<Token>();
        }

        public long Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Username { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<EnroledCourse> EnroledCourses { get; set; }
        public virtual ICollection<HistoryQuiz> HistoryQuizzes { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
