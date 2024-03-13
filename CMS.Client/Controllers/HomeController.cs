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
        private readonly CommonFunctions _commonFunctions;
        public HomeController(IConfiguration configuration, CommonFunctions commonFunctions)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:7149";
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
                DateTime expirationRefreshToken =
                    Convert.ToDateTime(HttpContext.Request.Cookies["refreshTokenExpires"].ToString());
                if (refreshToken != null && expirationRefreshToken > DateTime.UtcNow)
                {
                    var accessToken = HttpContext.Request.Cookies["accessToken"];
                    var expiresAccessToken = Convert.ToDateTime(HttpContext.Request.Cookies["accessTokenExpires"].ToString());
                    if (!string.IsNullOrEmpty(accessToken) && expiresAccessToken <= DateTime.UtcNow)
                    {
                        UserInfoTokenDTO u = new UserInfoTokenDTO();
                        u.AccessToken = accessToken;
                        u.RefreshToken = refreshToken;
                        var conn = BaseUrl + "/api/Authenticates/refreshToken";

                        var Res = await _commonFunctions.PostData(conn, JsonConvert.SerializeObject(u));
                        if (Res.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var responseData = await Res.Content.ReadAsStringAsync();
                            var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);

                            var data = JObject.Parse(jsonResponse.Data.ToString());
                            var newAccessToken = data["accessToken"].ToString();
                            var newRefreshToken = data["refreshToken"].ToString();
                            var expirationAccessToken = data["accessTokenExpires"].ToString();
                            var newExpirationRefreshToken = data["refreshTokenExpires"].ToString();

                            // Lưu thông tin người dùng vào session
                            HttpContext.Session.SetString("user", JsonConvert.SerializeObject(data["user"]));

                            // Set the new values for tokens
                            SetCookies("accessToken", newAccessToken, DateTime.MaxValue);
                            SetCookies("accessTokenExpires", expirationAccessToken, DateTime.MaxValue);
                            SetCookies("refreshToken", newRefreshToken, DateTime.MaxValue);
                            SetCookies("refreshTokenExpires", newExpirationRefreshToken, DateTime.MaxValue);
                        }
                    }
                }
                else
                {
                    Response.Cookies.Delete("accessToken");
                    Response.Cookies.Delete("refreshTokenExpires");
                    Response.Cookies.Delete("accessTokenExpires");
                    Response.Cookies.Delete("refreshToken");
                    HttpContext.Session.Remove("user");
                    return RedirectToAction("Login", "Authenticate");
                }
            }
            else
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshTokenExpires");
                Response.Cookies.Delete("accessTokenExpires");
                Response.Cookies.Delete("refreshToken");
            }
            return View();
        }

        public void SetCookies(string variable, string value, DateTime expires)
        {
            Response.Cookies.Append(variable, value,
                new CookieOptions
                {
                    Expires = expires,
                    HttpOnly = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                    Secure = true // Ensure cookie is sent only over HTTPS
                });
        }

    }
}