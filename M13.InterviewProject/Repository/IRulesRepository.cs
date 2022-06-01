namespace M13.InterviewProject.Repository
{
    using Models.Implementation;

    public interface IRulesRepository
    {
        void AddRule(Rule rule);
        void DeleteRule(string site);
        string GetRuleBySite(string site);
    }
}
