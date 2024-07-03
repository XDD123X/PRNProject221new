using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class Explode
    {
        public long Id { get; set; }
        public long? CourseId { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Course? Course { get; set; }
    }
}
