using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace CMS.Client.Controllers
{
    public class CourseController : Controller
    {
        private string DefaultCourseApiUrl = "";

        public CourseController()
        {
            DefaultCourseApiUrl = "https://localhost:7149/api/Courses";
        }

        public async Task<IActionResult> Index(int? filterType, string? key,
            string sortBy = "name", string sortValue = "asc", int page = 1)
        {
            string apiUrl = DefaultCourseApiUrl;

            apiUrl += $"?filterType={filterType ?? -1}";
            ViewBag.FilterType = filterType;

            if (!string.IsNullOrEmpty(key))
            {
                key = key.Trim();
                ViewBag.Key = key;
                apiUrl += apiUrl.Contains("?") ? "&" : "?";
                apiUrl += $"key={key}";
            }

            sortBy = sortBy.Trim();
            sortValue = sortValue.Trim();
            ViewBag.SortBy = sortBy;
            ViewBag.SortValue = sortValue;
            apiUrl += apiUrl.Contains("?") ? "&" : "?";
            apiUrl += $"sortBy=" + sortBy + "&sortValue=" + sortValue;
            ViewBag.Sort = $"sortBy=" + sortBy + "&sortValue=" + sortValue;


            var courses = new List<Course>();
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.GetAsync(apiUrl))
                {
                    using (HttpContent content = responseMessage.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        courses = JsonConvert.DeserializeObject<List<Course>>(data);
                    }
                }
            }

            int pageSize = 4;

            return View(courses.ToPagedList((int)page, (int)pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> RegisterAsync(int courseId)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Authenticate");
            }

            // Lấy thông tin người dùng từ session
            var user = JsonConvert.DeserializeObject<User>(userJson);

            Dictionary<string, string> notificationData = new Dictionary<string, string>();

            if (!user.Role.Equals("teacher"))
            {
                notificationData["Type"] = "alert-warning"; // hoặc "danger" tùy vào loại alert
                notificationData["Message"] = "You not have permission to register this course!";
                // Chuyển đổi Dictionary thành một chuỗi JSON để lưu vào session
                string notiJson = JsonConvert.SerializeObject(notificationData);

                // Lưu chuỗi JSON vào session
                HttpContext.Session.SetString("Notification", notiJson);
                return RedirectToAction("Index");
            }

            var courseRegistration = new RegistrationAddUpdateDTO
            {
                CourseId = courseId,
                UserId = user.UserId,
                RegistedTime = DateTime.Now
            };

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.PostAsJsonAsync(DefaultCourseApiUrl + "/Register", courseRegistration))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        notificationData["Type"] = "alert-danger"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Register failed. Please check again!";
                    }
                    else
                    {
                        notificationData["Type"] = "alert-success"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Registered success!";
                    }
                }
            }
            // Chuyển đổi Dictionary thành một chuỗi JSON để lưu vào session
            string notificationJson = JsonConvert.SerializeObject(notificationData);

            // Lưu chuỗi JSON vào session
            HttpContext.Session.SetString("Notification", notificationJson);

            return RedirectToAction("Index");
        }

    }
}
