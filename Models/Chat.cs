using System;
using System.Collections.Generic;

namespace operation_OLX.Models
{
    public partial class Chat
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? DateTime { get; set; }
    }
}
