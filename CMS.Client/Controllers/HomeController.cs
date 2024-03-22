using BusinessObject.DTO;
using BusinessObject.Models;
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
        public HomeController(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            _configuration = configuration;
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

        [HttpPost]
        public async Task<IActionResult> EditProfile(UserEditDTO userEditDTO)
        {
            Dictionary<string, string> notificationData = new Dictionary<string, string>();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.PutAsJsonAsync("https://localhost:7149/api/Users/" + userEditDTO.UserId, userEditDTO))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        notificationData["Type"] = "alert-danger";
                        if (responseMessage.Content != null)
                        {
                            string responseContent = await responseMessage.Content.ReadAsStringAsync();

                            // Đọc nội dung phản hồi và gán vào notificationData["Message"]
                            notificationData["Message"] = responseContent;
                        }
                        else
                        {
                            notificationData["Message"] = "Edit profile failed. Email existed!";
                        }
                    }
                    else
                    {
                        notificationData["Type"] = "alert-success";
                        notificationData["Message"] = "Edited success!";
                        var userInfoJson = HttpContext.Session.GetString("user");
                        if (!string.IsNullOrEmpty(userInfoJson))
                        {
                            var user = JsonConvert.DeserializeObject<User>(userInfoJson);
                            var loginModel = new LoginModel
                            {
                                Email = userEditDTO.Email,
                                Password = user.Password
                            };
                            // Tiến hành đăng nhập lại với thông tin mới
                            using (HttpResponseMessage loginResponse = await client.PostAsJsonAsync("https://localhost:7149/api/Authenticates/login", loginModel))
                            {
                                if (loginResponse.IsSuccessStatusCode)
                                {
                                    var responseData = await loginResponse.Content.ReadAsStringAsync();
                                    var jsonResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);

                                    var data = JObject.Parse(jsonResponse.Data.ToString());
                                    var accessToken = data["accessToken"].ToString();
                                    var refreshToken = data["refreshToken"].ToString();
                                    var expirationAccessToken = data["accessTokenExpires"].ToString();
                                    var expirationRefreshToken = data["refreshTokenExpires"].ToString();

                                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(data["user"]));

                                    SetCookies("accessToken", accessToken, Convert.ToDateTime(expirationAccessToken));
                                    SetCookies("refreshToken", refreshToken, DateTime.MaxValue);
                                    SetCookies("refreshTokenExpires", expirationRefreshToken, DateTime.MaxValue);
                                }
                            }
                        }
                    }

                    string notificationJson = JsonConvert.SerializeObject(notificationData);

                    // Lưu chuỗi JSON vào session
                    HttpContext.Session.SetString("Notification", notificationJson);

                    var historyUrl = HttpContext.Session.GetString("historyUrl");
                    if (!string.IsNullOrEmpty(historyUrl))
                    {
                        return Redirect(historyUrl);
                    }
                    else
                    {
                        historyUrl = "/";
                        HttpContext.Session.SetString("historyUrl", historyUrl);
                        return Redirect(historyUrl);
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
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
                });
        }

    }
}