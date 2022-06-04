namespace M13.InterviewProject.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ISpellChecker
    {
        Task<ISpellCheckError[]> GetErrors(string text);
    }
}
