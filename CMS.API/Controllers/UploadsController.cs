using BusinessObject.DTO;
using BusinessObject.Models;
using Infrastructures.Interfaces;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUploadRepository _uploadRepository;

        public UploadsController(IConfiguration configuration, IUploadRepository uploadRepository)
        {
            _configuration= configuration;
            _uploadRepository= uploadRepository;
        }

        // POST: api/Uploads/CreateUpload
        [HttpPost("CreateUpload")]
        public async Task<ActionResult<Course>> CreateUpload([FromBody] UploadAddUpdateDTO courseData)
        {
            try
            {
                if (courseData == null)
                {
                    return BadRequest("Invalid course data");
                }            

                // Create a new Course object
                var newCourse = new Upload
                {
                    RegistrationId = courseData.RegistrationId,
                    FileId = courseData.FileId,
                    UploadName = courseData.UploadName,
                    UploadDescription = courseData.UploadDescription,
                    UploadTime = DateTime.UtcNow,
                    IsHide= courseData.IsHide           
                };

                var status = await _uploadRepository.AddUpload(newCourse);

                return Ok(status);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error creating course: " + ex.Message);
            }
        }
        // GET: api/Uploads/GetUploadsByRegistrationId/{registrationId}
        [HttpGet("GetUploadsByRegistrationId/{registrationId}")]
        public async Task<ActionResult<IEnumerable<Upload>>> GetUploadsByRegistrationId(int registrationId)
        {
            try
            {
                var uploads = await _uploadRepository.GetUploadsByRegistrationId(registrationId);

                if (uploads == null || !uploads.Any())
                {
                    return NotFound("No uploads found for the given registration ID.");
                }

                return Ok(uploads);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving uploads: " + ex.Message);
            }
        }

        // GET: api/Uploads/{uploadId}
        [HttpGet("{uploadId}")]
        public async Task<ActionResult<Upload>> GetUploadById(int uploadId)
        {
            try
            {
                var upload = await _uploadRepository.GetUploadById(uploadId);

                if (upload == null)
                {
                    return NotFound("Upload not found for the given upload ID.");
                }

                return Ok(upload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving upload: " + ex.Message);
            }
        }

        // PUT: api/Uploads/UpdateUpload
        [HttpPut("UpdateUpload")]
        public async Task<IActionResult> UpdateUpload([FromBody] UploadAddUpdateDTO uploadAddUpdateDTO)
        {
            try
            {
                // Gọi phương thức từ repository để cập nhật thông tin tải lên
                var result = await _uploadRepository.UpdateUpload(uploadAddUpdateDTO);

                if (result)
                {
                    return Ok("Upload updated successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to update upload. Please check again.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating upload: " + ex.Message);
            }
        }



    }
}
