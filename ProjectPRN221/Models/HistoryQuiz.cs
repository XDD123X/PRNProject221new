using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class HistoryQuiz
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? QuizzId { get; set; }
        public int? Answer { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Quiz? Quizz { get; set; }
        public virtual User? User { get; set; }
    }
}
