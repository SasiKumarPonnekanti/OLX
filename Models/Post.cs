using System;
using System.Collections.Generic;

namespace operation_OLX.Models
{
    public partial class Post
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
        public string State { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? LandMark { get; set; }
        public string? ImagePath1 { get; set; }
        public string? ImagePath2 { get; set; }
        public string? ImagePath3 { get; set; }
        public string Status { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime? Dateposted { get; set; }
    }
}
