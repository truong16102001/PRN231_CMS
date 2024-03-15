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

    }
}
