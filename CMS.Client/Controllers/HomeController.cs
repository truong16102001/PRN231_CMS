using BusinessObject.DTO;
using BusinessObject.Models;
using CMS.Client.Commons;
using CMS.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace CMS.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient client = null;
        private string BaseUrl = "";
        private string DefaultAuthenticatesApiUrl = "";
        private readonly CommonFunctions _commonFunctions;
        public HomeController(IConfiguration configuration, CommonFunctions commonFunctions)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:7149";
            DefaultAuthenticatesApiUrl = "https://localhost:7149/api/Authenticates";
            _configuration = configuration;
            _commonFunctions = commonFunctions;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userInfoJson = HttpContext.Session.GetString("user");
            if (!string.IsNullOrEmpty(userInfoJson))
            {
                var refreshToken = HttpContext.Request.Cookies["refreshToken"];
                DateTime expirationRefreshToken = Convert.ToDateTime(HttpContext.Request.Cookies["refreshTokenExpires"].ToString());
                if (refreshToken != null && expirationRefreshToken > DateTime.Now)
                {
                    var accessToken = HttpContext.Request.Cookies["accessToken"];
                    DateTime expirationAccessToken = Convert.ToDateTime(HttpContext.Request.Cookies["accessTokenExpires"].ToString());
                    if(accessToken != null && expirationAccessToken < DateTime.Now)
                    {
                        UserInfoTokenDTO u = new UserInfoTokenDTO();
                        u.RefreshToken = refreshToken;
                        u.AccessToken = accessToken;
                        var conn = $"api/Authenticates/refreshToken";
                        var Res = await _commonFunctions.PostData(conn, JsonConvert.SerializeObject(u));
                        if (Res.IsSuccessStatusCode)
                        {
                            var responseData = await Res.Content.ReadAsStringAsync();
                            var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);


                            var data = JObject.Parse(jsonResponse.Data.ToString());
                            var newAccessToken = data["accessToken"].ToString();
                            var newRefreshToken = data["refreshToken"].ToString();
                            var newExpirationAccessToken = data["accessTokenExpires"].ToString();
                            var newExpirationRefreshToken = data["refreshTokenExpires"].ToString();


                            // Lưu thông tin người dùng vào session
                            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(data["user"]));

                            SetCookies("accessToken", newAccessToken);
                            SetCookies("refreshToken", newRefreshToken);
                            SetCookies("accessTokenExpires", newExpirationAccessToken.ToString());
                            SetCookies("refreshTokenExpires", newExpirationRefreshToken.ToString());

                        }
                    }
                    
                }
                else
                {
                    Response.Cookies.Delete("accessToken");
                    Response.Cookies.Delete("accessTokenExpires");
                    Response.Cookies.Delete("refreshTokenExpires");
                    Response.Cookies.Delete("refreshToken");
                    HttpContext.Session.Remove("user");
                    return RedirectToAction("Login", "Authenticate");
                }
            }
            return View();
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
    }
}