namespace M13.InterviewProject.Repository.Implementation
{
    using System.Collections.Generic;
    using Models.Implementation;

    public class RulesRepository : IRulesRepository
    {
        private readonly Dictionary<string, string> _rules = new();

        public void AddRule(Rule rule)
        {
            lock (_rules)
            {
                if (_rules.ContainsKey(rule.Site))
                {
                    _rules.Remove(rule.Site);
                }

                _rules.Add(rule.Site, rule.Value);
            }
        }

        public void DeleteRule(string site)
        {
            lock (_rules)
            {
                _rules.Remove(site);
            }
        }

        public string GetRuleBySite(string site)
        {
            lock (_rules)
            {
                if (_rules.ContainsKey(site))
                {
                    return _rules[site];
                }
                throw new KeyNotFoundException($"Не найдено правило для сайта {site}");
            }
        }
    }
}
