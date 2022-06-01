namespace M13.InterviewProject.Models.Implementation
{
    public class Rule
    {
        public Rule(string site, string value)
        {
            Site = site;
            Value = value;
        }

        public string Site { get; }
        public string Value { get; }
    }
}
