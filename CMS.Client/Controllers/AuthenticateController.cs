using BusinessObject.DTO;
using BusinessObject.Models;
using CMS.Client.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace CMS.Client.Controllers
{
    public class AuthenticateController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient client = null;
        private string BaseUrl = "";
        private readonly CommonFunctions _commonFunctions;
        public AuthenticateController(IConfiguration configuration, CommonFunctions commonFunctions)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:7149";
            _configuration = configuration;
            _commonFunctions = commonFunctions;
        }

        [HttpGet]
        public IActionResult Login()
        {
         
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            LoginModel us = new LoginModel();
            us.Email = email;
            us.Password = password;
            var response = _commonFunctions.PostData("api/Authenticates/login", JsonConvert.SerializeObject(us));

            if (response.Result.IsSuccessStatusCode)
            {
                var responseData = await response.Result.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);


                var data = JObject.Parse(jsonResponse.Data.ToString());
                var accessToken = data["accessToken"].ToString();
                var refreshToken = data["refreshToken"].ToString();
                var expirationAccessToken = data["accessTokenExpires"].ToString();
                var expirationRefreshToken = data["refreshTokenExpires"].ToString();


                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(data["user"]));

                SetCookies("accessToken", accessToken);
                SetCookies("refreshToken", refreshToken);
                SetCookies("accessTokenExpires", expirationAccessToken.ToString());
                SetCookies("refreshTokenExpires", expirationRefreshToken.ToString());

                return RedirectToAction("Index", "Home");

            }
            else
            {
                return RedirectToAction("Index", "Home", new { @alertMessage = "Login fail!" });
            }
        }

        public void SetCookies(string variable, string value)
        {
            Response.Cookies.Append(variable, value,
                new CookieOptions
                {
                    Expires = DateTime.MaxValue,
                    HttpOnly = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
                });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("accessTokenExpires");
            Response.Cookies.Delete("refreshTokenExpires");
            Response.Cookies.Delete("refreshToken");
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }
    }
}
