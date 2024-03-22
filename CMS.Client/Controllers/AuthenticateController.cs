using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
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
        public AuthenticateController(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _configuration = configuration;
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

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.PostAsJsonAsync("https://localhost:7149/api/Authenticates/login", us))
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        var responseData = await responseMessage.Content.ReadAsStringAsync();
                        var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);


                        var data = JObject.Parse(jsonResponse.Data.ToString());
                        var accessToken = data["accessToken"].ToString();
                        var refreshToken = data["refreshToken"].ToString();
                        var expirationAccessToken = data["accessTokenExpires"].ToString();
                        var expirationRefreshToken = data["refreshTokenExpires"].ToString();

                        // Lưu thông tin người dùng vào session
                        HttpContext.Session.SetString("user", JsonConvert.SerializeObject(data["user"]));

                        SetCookies("accessToken", accessToken, Convert.ToDateTime(expirationAccessToken));
                        SetCookies("refreshToken", refreshToken, DateTime.MaxValue);
                        SetCookies("refreshTokenExpires", expirationRefreshToken, DateTime.MaxValue);

                        var historyUrl = HttpContext.Session.GetString("historyUrl");
                        if (!string.IsNullOrEmpty(historyUrl))
                        {
                            return Redirect(historyUrl);
                        }
                        historyUrl = "/";
                        HttpContext.Session.SetString("historyUrl", historyUrl);

                        return Redirect(historyUrl);
                    }
                    else
                    {
                        Dictionary<string, string> notificationData = new Dictionary<string, string>();
                        notificationData["Type"] = "alert-danger"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Login failed! Invalid email or password";
                        // Chuyển đổi Dictionary thành một chuỗi JSON để lưu vào session
                        string notificationJson = JsonConvert.SerializeObject(notificationData);

                        // Lưu chuỗi JSON vào session
                        HttpContext.Session.SetString("Notification", notificationJson);

                        return Redirect("/Authenticate/Login");
                    }
                }
            }
        }

        public void SetCookies(string variable, string value, DateTime expires)
        {

            Response.Cookies.Append(variable, value,
                new CookieOptions
                {
                    Expires = expires,
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict
                });
        }


        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshTokenExpires");
            Response.Cookies.Delete("refreshToken");
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "Home");
        }
    }
}
