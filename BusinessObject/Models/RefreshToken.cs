﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class RefreshToken
    {
        public int UserId { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
