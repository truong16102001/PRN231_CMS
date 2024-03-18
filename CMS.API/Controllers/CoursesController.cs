using BusinessObject.DTO;
using BusinessObject.Models;
using Infrastructures.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICourseRepository _courseRepository;
        public CoursesController(IConfiguration configuration, IRegistrationRepository registrationRepository, ITokenService tokenService, ICourseRepository courseRepository)
        {
            _configuration = configuration;
            _registrationRepository = registrationRepository;
            _courseRepository = courseRepository;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(int? filterType, string? key,
            string? sortBy, string? sortValue)
        {
            try
            {
                IEnumerable<Course> courses = await _courseRepository.GetCourses();

                if (filterType != null && filterType != -1)
                {
                    IEnumerable<CourseRegistration> registeredCourses = await _registrationRepository.GetCourseRegistrations();

                    // Lọc ra các bản ghi có UserId tương ứng với filterType
                    var userRegistrations = registeredCourses.Where(cr => cr.UserId == filterType);


                    //Lọc các course của user có userId = filterType
                    courses = courses.Where(c => userRegistrations.Any(r => r.CourseId == c.CourseId));

                }

                if (!string.IsNullOrEmpty(key))
                {
                    courses = courses.Where(c => (c.CourseName.Contains(key, StringComparison.OrdinalIgnoreCase)) || (c.CourseDescription.Contains(key, StringComparison.OrdinalIgnoreCase))).ToList();
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "name":
                            if (!string.IsNullOrEmpty(sortValue) && sortValue.ToLower().Equals("desc"))
                                courses = courses.OrderByDescending(c => c.CourseName).ToList();
                            else
                                courses = courses.OrderBy(c => c.CourseName).ToList();
                            break;
                        default:
                            // Mặc định sắp xếp theo tên nếu không có tiêu chí sắp xếp hợp lệ
                            courses = courses.OrderBy(c => c.CourseName).ToList();
                            break;
                    }
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Trả về lỗi 500 nếu xảy ra lỗi
                return StatusCode(500, "Error retrieving courses: " + ex.Message);
            }
        }

        // GET: api/Courses/{CourseId}
        [HttpGet("{CourseId}")]
        public async Task<ActionResult<Course>> GetCourseById(int CourseId)
        {
            try
            {
                var course = await _courseRepository.GetCourseById(CourseId);

                if (course == null)
                {
                    return NotFound(); 
                }

                return Ok(course); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving course: " + ex.Message);
            }
        }


        [HttpPost("Register")]
        public async Task<ActionResult<CourseRegistration>> RegisterCourse([FromBody] RegistrationAddUpdateDTO courseRegistration)
        {
            try
            {
                if (courseRegistration == null)
                {
                    return BadRequest("Invalid courseRegistration data");
                }
                return StatusCode(200, await _registrationRepository.RegisterCourse(courseRegistration));
            }
            catch (ApplicationException ae)
            {
                return StatusCode(400, ae.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
