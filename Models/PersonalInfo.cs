using System;
using System.Collections.Generic;

namespace operation_OLX.Models
{
    public partial class PersonalInfo
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? AboutMe { get; set; }
        public string? Phone { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? LandMark { get; set; }
    }
}
