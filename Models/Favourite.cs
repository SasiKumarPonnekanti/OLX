using System;
using System.Collections.Generic;

namespace operation_OLX.Models
{
    public partial class Favourite
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int PostId { get; set; }
    }
}
