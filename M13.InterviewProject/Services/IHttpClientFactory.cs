namespace M13.InterviewProject.Services
{
    using System.Net.Http;

    public interface IHttpClientFactory
    {
        HttpClient CreateClient();
    }
}
