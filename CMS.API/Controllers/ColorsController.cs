using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public ColorsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET: api/Colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetColors()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage responseMessage = await client.GetAsync("https://csscolorsapi.com/api/colors"))
                    {
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            string data = await responseMessage.Content.ReadAsStringAsync();
                            var colorResponse = JsonConvert.DeserializeObject<ColorResponseDTO>(data);
                            return Ok(colorResponse.Colors);
                        }
                        else
                        {
                            return StatusCode((int)responseMessage.StatusCode, "Error retrieving colors from external API");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving colors: " + ex.Message);
            }
        }
    }

}
