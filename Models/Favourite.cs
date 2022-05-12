using System;
using System.Collections.Generic;

namespace operation_OLX.Models
{
    public partial class Favourite
    {
        //Model That Save The Post Id of the post favorited by user with UserId as Key
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int PostId { get; set; }
    }
}
