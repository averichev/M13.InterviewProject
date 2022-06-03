namespace M13.InterviewProject.Services
{
    using System.Threading.Tasks;

    public interface IHtmlReader
    {
        Task<string> ReadPage(string page);
    }
}
