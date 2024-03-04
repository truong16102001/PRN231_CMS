using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims);
         string GenerateRefreshToken();

         ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

         Task<RefreshToken> GetRefreshTokenByUserId(int userId);

        Task SaveRefreshToken(int userid, string refreshToken, DateTime expires);

    }
}
