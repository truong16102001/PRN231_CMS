using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        /// <summary>
        /// API data 
        /// </summary>
        public object Data { get; set; }
        /// <summary>
		/// api status Code
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        ///  message
        /// </summary>
        public string Message { get; set; }
    }
}
