using BusinessObject.DTO;
using BusinessObject.Models;
using CMS.Client.Commons;
using CMS.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
        private readonly CommonFunctions _commonFunctions;
        public HomeController(IConfiguration configuration, CommonFunctions commonFunctions)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
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
                if (refreshToken != null && expirationRefreshToken > DateTime.Now)
                {
                    var accessToken = HttpContext.Request.Cookies["accessToken"];
                    if (string.IsNullOrEmpty(accessToken))
                    {
                        var conn = "https://localhost:7149/api/Authenticates/refreshToken";

                        using (HttpClient client = new HttpClient())
                        {
                            UserInfoTokenDTO userInfoTokenDTO = new UserInfoTokenDTO();
                            userInfoTokenDTO.RefreshToken = refreshToken;
                            using (HttpResponseMessage responseMessage = await client.PostAsJsonAsync(conn, userInfoTokenDTO))
                            {
                                if (responseMessage.IsSuccessStatusCode)
                                {
                                    var responseData = await responseMessage.Content.ReadAsStringAsync();
                                    var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);

                                    var data = JObject.Parse(jsonResponse.Data.ToString());
                                    var newAccessToken = data["accessToken"].ToString();
                                    var newRefreshToken = data["refreshToken"].ToString();
                                    var expirationAccessToken = data["accessTokenExpires"].ToString();
                                    var newExpirationRefreshToken = data["refreshTokenExpires"].ToString();

                                 
                                    // Lưu thông tin người dùng vào session
                                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(data["user"]));

                                    // Set the new values for tokens
                                    SetCookies("accessToken", newAccessToken, Convert.ToDateTime(expirationAccessToken));
                                    SetCookies("refreshToken", newRefreshToken, DateTime.MaxValue);
                                    SetCookies("refreshTokenExpires", newExpirationRefreshToken, DateTime.MaxValue);
                                }
                                else
                                {
                                    Console.WriteLine("refreshToken error from Home");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Access Token Not expired - From Home");
                    }
                }
                else
                {
                    Response.Cookies.Delete("accessToken");
                    Response.Cookies.Delete("refreshTokenExpires");
                    Response.Cookies.Delete("refreshToken");
                    HttpContext.Session.Remove("user");
                    return RedirectToAction("Login", "Authenticate");
                }
            }
            else
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshTokenExpires");
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
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
                });
        }

    }
}