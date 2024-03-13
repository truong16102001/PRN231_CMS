using BusinessObject.DTO;
using BusinessObject.Models;
using Infrastructures.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticatesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public static UserInfoTokenDTO userInfo = new();

        public AuthenticatesController(IConfiguration configuration, IUserRepository userAuthentication, ITokenService tokenService)
        {
            _configuration = configuration;
            _userRepository = userAuthentication;
            _tokenService = tokenService;
        }

        /// <summary>	
		/// revevice email and password and check is login info is valid or not
		/// </summary>
		/// <param name="model">login model contain email and password</param>
		/// <returns>access token and refresh token</returns>
		///  created by: Nguyễn Đình Trường
		///  created at: 2024/26/2
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel request)
        {
            if (request is null) return BadRequest();
            var user = await _userRepository.Login(request);

            if (user is null)
            {
                return StatusCode(404, new ApiResponse
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "Invalid email or password"
                });
            }
            userInfo.User = user;

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                     new Claim(ClaimTypes.Email, user.Email!),
                     new Claim("UserId", user.UserId.ToString())
            };

            var accesstoken = _tokenService.GenerateAccessToken(authClaims);

            DateTime expiresAccessToken = accesstoken.ValidTo;


            //Handle when refreshtoken existed
            var existedToken = await _tokenService.GetRefreshTokenByUserId(user.UserId);

            string newRefreshToken = "";
            DateTime expiredTime = DateTime.UtcNow;

            if (existedToken == null || string.IsNullOrEmpty(existedToken.Token) || existedToken.ExpirationTime <= DateTime.Now)
            {
                newRefreshToken = _tokenService.GenerateRefreshToken();
                expiredTime = DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:RefreshTokenValidityInMinutes"]));
                await _tokenService.SaveRefreshToken(existedToken.UserId, newRefreshToken, expiredTime);
            }
            else
            {
                newRefreshToken = existedToken.Token;
                expiredTime = (DateTime)existedToken.ExpirationTime;
            }

            userInfo.AccessToken = new JwtSecurityTokenHandler().WriteToken(accesstoken);
            userInfo.RefreshToken = newRefreshToken;
            userInfo.RefreshTokenExpires = expiredTime;
            userInfo.AccessTokenExpires = expiresAccessToken;

            return StatusCode(200, new ApiResponse
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = userInfo,
                Message = "Authenticated success"
            });
        }

        /// <summary>	
		/// create new access token and refresh token bass current tokens
        /// Chỉ lấy 1 refreshtoken mới khi refreshtoken cũ đã hết hạn
		/// </summary>
		/// <param name="userinfo">userinfo contain detail of logged user and old access token and refresh token</param>
		/// <returns>register success or not</returns>
		///  created by: Nguyễn Đình Trường
		///  created at: 2024/26/2
		[HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken(UserInfoTokenDTO userLoginInfo)
        {
            if (userLoginInfo is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = userLoginInfo.AccessToken;
            string? refreshToken = userLoginInfo.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken ?? userInfo.AccessToken);

            string userId = principal.FindFirst("UserId").Value;


            var existedToken = await _tokenService.GetRefreshTokenByUserId(int.Parse(userId));
            // invalid user || invalid refresh token
            if (existedToken == null || existedToken.Token != refreshToken)
            {
                return BadRequest("Refresh Token is not existed in db or token is expired");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
            string newRefreshToken = existedToken.Token;
            DateTime expiredTime = (DateTime)existedToken.ExpirationTime;

            if (expiredTime <= DateTime.UtcNow)
            {
                newRefreshToken = _tokenService.GenerateRefreshToken();
                expiredTime = DateTime.Now.AddMinutes(int.Parse(_configuration["JWT:RefreshTokenValidityInMinutes"]));
                await _tokenService.SaveRefreshToken(existedToken.UserId, newRefreshToken, expiredTime);
            }
            userInfo.RefreshToken = newRefreshToken;
            userInfo.AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            userInfo.RefreshTokenExpires = expiredTime;
            userInfo.AccessTokenExpires = newAccessToken.ValidTo;

            return StatusCode(200, new ApiResponse
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Data = userInfo,
                Message = "Refresh token success"
            });
        }
    }
}
