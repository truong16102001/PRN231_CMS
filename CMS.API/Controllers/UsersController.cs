using BusinessObject.DTO;
using Infrastructures.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepo;

        public UsersController(IConfiguration configuration, IUserRepository userRepo)
        {
            _configuration = configuration;
            _userRepo = userRepo;
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserEditDTO userEditDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (userId != userEditDTO.UserId)
                {
                    return BadRequest("UserId in the path does not match UserId in the request body.");
                }

                // Thực hiện cập nhật thông tin cá nhân của người dùng trong hệ thống
                var status = await _userRepo.UpdateUserAsync(userEditDTO);

                if (!status)
                {
                    return BadRequest($"{userEditDTO.Email} existed");
                }

                return Ok("User profile updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating user profile: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserAddEditDTO userEditDTO)
        {
            try
            {
               
                // Thực hiện cập nhật thông tin cá nhân của người dùng trong hệ thống
                var status = await _userRepo.AddUserAsync(userEditDTO);

                if (!status)
                {
                    return BadRequest($"{userEditDTO.Email} existed");
                }

                Console.WriteLine("lalaa");
                return Ok("User profile updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating user profile: " + ex.Message);
            }
        }

        [HttpGet("CheckEmailExist")]
        public async Task<IActionResult> CheckEmailExist(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Email is required.");
                }

                // Kiểm tra xem email đã tồn tại trong hệ thống hay chưa
                bool emailExists = await _userRepo.EmailExistsAsync(email);

                // Trả về kết quả kiểm tra
                return Ok(emailExists);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while checking email existence: " + ex.Message);
            }
        }
    }
}
