using System.Net.Http;

namespace tsui.Library
{
    public class ServiceRequestHelpers
    {
        public static HttpRequestMessage CreateGetRequestObject(string endpoint)
        {
            return new HttpRequestMessage(HttpMethod.Get, endpoint);
        }

        public static HttpRequestMessage CreatePostRequestObject(string endpoint)
        {
            return new HttpRequestMessage(HttpMethod.Post, endpoint);
        }

    }
}
