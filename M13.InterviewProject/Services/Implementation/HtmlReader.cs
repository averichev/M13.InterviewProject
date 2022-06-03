namespace M13.InterviewProject.Services.Implementation
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using HtmlAgilityPack;
    using Repository;

    public class HtmlReader : IHtmlReader
    {
        private readonly IRulesRepository _rulesRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public HtmlReader(IRulesRepository rulesRepository, IHttpClientFactory httpClientFactory)
        {
            _rulesRepository = rulesRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> ReadPageAsync(string page)
        {
            try
            {
                var content = await GetPageContent(page);
                var selector = _rulesRepository.GetRuleBySite(page);
                var text = ReadHtml(content, selector);
                return text;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }

        }

        private async Task<string> GetPageContent(string page)
        {
            var response = await _httpClientFactory.CreateClient().GetAsync("https://" + page);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private static string ReadHtml(string html, string rule)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var node = document.DocumentNode;
            var nodes = node.SelectNodes(rule);
            if (nodes == null)
            {
                throw new RankException($"Не найдены узлы {rule} в теле страницы");
            }

            var result = nodes.Aggregate("", GetString);
            return result;
        }

        private static string GetString(string current, HtmlNode node) => current + "\r\n" + node.InnerText;
    }
}
