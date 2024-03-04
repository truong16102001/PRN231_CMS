using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess
{
    public class RefreshTokenDAO
    {
        public static async Task<RefreshToken> GetRefreshTokenByUserIdAsync(int userid)
        {
            RefreshToken? refreshToken;
            using (var context = new PRN231_DemoCMSContext())
            {
                refreshToken = await context.RefreshTokens.Include(r => r.User).Where(a => a.UserId == userid).FirstOrDefaultAsync();
            }
            return refreshToken ?? new();
        }

        public static async Task SaveToken(int userId, string refreshToken, DateTime expires)
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                var existingToken = await context.RefreshTokens.FirstOrDefaultAsync(a => a.UserId == userId);
                if (existingToken != null)
                {
                    // Nếu refresh token đã tồn tại, cập nhật thông tin của nó
                    existingToken.Token = refreshToken;
                    existingToken.ExpirationTime = expires;
                }
                else
                {
                    // Nếu refresh token chưa tồn tại, thêm mới một refresh token
                    var newToken = new RefreshToken
                    {
                        UserId = userId,
                        Token = refreshToken,
                        ExpirationTime = expires
                    };
                    context.RefreshTokens.Add(newToken);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
