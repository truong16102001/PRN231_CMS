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
    }
}
