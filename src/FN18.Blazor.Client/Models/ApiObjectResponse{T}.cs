using System.Net;

namespace FN18.Blazor.Client.Models
{
    public class ApiObjectResponse<T> : ApiResponse
    {
        public T Response { get; set; }
    }

    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiError Error { get; set; }

    }
}
