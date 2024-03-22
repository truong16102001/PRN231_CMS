using BusinessObject.Models;
using Infrastructures.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICourseRepository _courseRepository;
        public RegistrationsController(IConfiguration configuration, IRegistrationRepository registrationRepository, ICourseRepository courseRepository)
        {
            _configuration = configuration;
            _registrationRepository = registrationRepository;
            _courseRepository = courseRepository;
        }

        // GET: api/Registrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetRegistrationsByCourseOrUser(int? courseId, int? userId)
        {
            try
            {
                IEnumerable<CourseRegistration> courses = await _registrationRepository.GetCourseRegistrations();
                         
                if(courseId != null)
                {
                    courses = courses.Where(c => c.CourseId == courseId).ToList();
                }

                if(userId != null)
                {
                    courses = courses.Where(c => c.UserId == userId).ToList();
                }
                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Trả về lỗi 500 nếu xảy ra lỗi
                return StatusCode(500, "Error retrieving courses: " + ex.Message);
            }
        }

        // GET: api/Registrations
        [HttpGet("{RegistrationId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetRegistrationsById(int RegistrationId)
        {
            try
            {
                IEnumerable<CourseRegistration> courses = await _registrationRepository.GetCourseRegistrations();

                if (RegistrationId != null)
                {
                    courses = courses.Where(c => c.RegistrationId == RegistrationId).ToList();
                }    
                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Trả về lỗi 500 nếu xảy ra lỗi
                return StatusCode(500, "Error retrieving courses: " + ex.Message);
            }
        }
    }
}
