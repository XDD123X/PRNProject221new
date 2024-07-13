using System;
using System.Collections.Generic;

namespace ProjectPRN221.Models
{
    public partial class Token
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Type { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Content { get; set; }
        public string? Email { get; set; }

        public virtual User? User { get; set; }
    }
}