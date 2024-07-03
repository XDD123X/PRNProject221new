using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class Quiz
    {
        public long Id { get; set; }
        public long? CourseId { get; set; }
        public string? Question { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public int? Answer { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Course? Course { get; set; }
    }
}
