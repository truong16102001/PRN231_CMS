﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
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

        [JsonIgnore]
        public virtual RefreshToken? RefreshToken { get; set; }
        [JsonIgnore]
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
