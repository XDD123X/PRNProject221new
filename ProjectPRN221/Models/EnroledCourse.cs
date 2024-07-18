using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class EnroledCourse
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? CourseId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }
    }
}
