using Newtonsoft.Json;
// constructing a meaningful response object
namespace School.API.Data.Responses
{
    // structured error response
    public class ErrorResponseData
    {
        // error response structure
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}