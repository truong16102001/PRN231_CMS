﻿using System;
using System.Collections.Generic;

namespace BussinessObject1.Models
{
    public partial class User
    {
        public User()
        {
            CourseRegistrations = new HashSet<CourseRegistration>();
        }

        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }

        public virtual RefreshToken? RefreshToken { get; set; }
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
