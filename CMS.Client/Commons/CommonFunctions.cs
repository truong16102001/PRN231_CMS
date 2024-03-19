using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace CMS.Client.Commons
{
    public  class CommonFunctions : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient client = null;
        private static string BaseUrl = "";
       

        public CommonFunctions(IConfiguration configuration)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BaseUrl = "https://localhost:7149";
            _configuration = configuration;
        }

       
        public  async Task<HttpResponseMessage> PostData(string targerAddress, string content)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var Response = await client.PostAsync(targerAddress, new StringContent(content, Encoding.UTF8, "application/json"));
            return Response;
        }
       



    }
}
