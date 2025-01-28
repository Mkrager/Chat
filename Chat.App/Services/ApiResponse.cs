using System.Net;

namespace Chat.App.Services
{
    public class ApiResponse
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();
        }
    }
}
