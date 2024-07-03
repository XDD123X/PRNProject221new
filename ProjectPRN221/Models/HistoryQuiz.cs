using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class HistoryQuiz
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? CourseId { get; set; }
        public int? Answer { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }
    }
}
