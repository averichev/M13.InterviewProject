using System.Net;
using System.Net.Http;

namespace M13.InterviewProject.Services.Implementation
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient() =>
            new(
                new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    AutomaticDecompression = DecompressionMethods.Deflate
                });
    }
}
