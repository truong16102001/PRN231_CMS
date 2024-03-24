using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.Client.Controllers
{
    public class UploadController : Controller
    {
        private string DefaultUploadApiUrl = "";

        public UploadController()
        {
            DefaultUploadApiUrl = "https://localhost:7149/api/Uploads";
        }

        public async Task<IActionResult> DetailsAsync(int uploadId)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Authenticate");
            }

            Upload upload = new Upload();
            using (HttpClient client = new HttpClient())
            {
                string url = DefaultUploadApiUrl + "/" + uploadId;
                Console.WriteLine(url);
                using (HttpResponseMessage responseMessage = await client.GetAsync(url))
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        // Đọc dữ liệu từ phản hồi
                        var responseData = await responseMessage.Content.ReadAsStringAsync();

                        // Chuyển đổi dữ liệu từ JSON sang đối tượng Upload
                        upload = JsonConvert.DeserializeObject<Upload>(responseData);
                    }
                    else
                    {
                        Console.WriteLine("Looix ");
                    }
                }
            }
            return View(upload);
        }

        public IActionResult Add(int RegistrationId)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Authenticate");
            }
            ViewBag.RegistrationId = RegistrationId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UploadAddUpdateDTO uploadAddUpdateDTO)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Authenticate");
            }

            Dictionary<string, string> notificationData = new Dictionary<string, string>();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.PutAsJsonAsync(DefaultUploadApiUrl + "/UpdateUpload", uploadAddUpdateDTO))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        notificationData["Type"] = "alert-danger"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Update Upload failed. Please check again!";
                    }
                    else
                    {
                        notificationData["Type"] = "alert-success"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Update Upload success!";
                    }
                }
            }
            // Chuyển đổi Dictionary thành một chuỗi JSON để lưu vào session
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
                return Redirect("https://localhost:7149/api/Course");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(UploadAddUpdateDTO uploadAddUpdateDTO)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            var userJson = HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userJson))
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Authenticate");
            }

            Dictionary<string, string> notificationData = new Dictionary<string, string>();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage responseMessage = await client.PostAsJsonAsync(DefaultUploadApiUrl + "/CreateUpload", uploadAddUpdateDTO))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        notificationData["Type"] = "alert-danger"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Create Upload failed. Please check again!";
                    }
                    else
                    {
                        notificationData["Type"] = "alert-success"; // hoặc "danger" tùy vào loại alert
                        notificationData["Message"] = "Create Upload success!";
                    }
                }
            }
            // Chuyển đổi Dictionary thành một chuỗi JSON để lưu vào session
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
                return Redirect("https://localhost:7149/api/Course");
            }
        }
    }
}
