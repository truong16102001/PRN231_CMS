using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class UserInfoTokenDTO
    {
        public User? User { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime? AccessTokenExpires { get; set; }

        public DateTime? RefreshTokenExpires { get; set; }
    }
}
