using System;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace M13.InterviewProject.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Models.Implementation;
    using Repository;
    using Services;

    /// <summary>
    /// sample usage:
    /// 1) check xpath rule '//ol' for site yandex.ru: http://localhost:56660/api/rules/add?site=yandex.ru&rule=%2f%2fol
    /// 2) check rule is saved:  http://localhost:56660/api/rules/get?site=yandex.ru
    /// 3) view text parsed by rule: http://localhost:56660/api/rules/test?page=yandex.ru
    /// 4) view errors in text: http://localhost:56660/api/spell/errors?page=yandex.ru
    /// 5) view errors count in text: http://localhost:56660/api/spell/errorscount?page=yandex.ru
    /// </summary>
    [Route("api/rules")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRulesRepository _rulesRepository;

        public ValuesController(
            IHttpClientFactory clientFactory,
            IRulesRepository rulesRepository,
            ILogger<ValuesController> logger
        )
        {
            _clientFactory = clientFactory;
            _rulesRepository = rulesRepository;
            _logger = logger;
        }

        [HttpGet("add")]
        public void AddRule(string site, string rule)
        {
            var newRule = new Rule(site, rule);
            _rulesRepository.AddRule(newRule);
        }

        [HttpGet("get")]
        public IActionResult GetRules(string site)
        {
            string s;
            try
            {
                s = _rulesRepository.GetRuleBySite(site);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestRules(string page, string rule = null)
        {
            //var site = new Uri("http://" + page).Host;
            var response = await _clientFactory.CreateClient().GetAsync("http://" + page);
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug(content);
            var selector = _rulesRepository.GetRuleBySite(page);
            var text = ReadHtml(content, selector);
            return Ok(text);
        }

        private static string ReadHtml(string html, string rule)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var nodes = document.DocumentNode.SelectNodes(rule);
            Func<string, HtmlNode, string> func = GetString;
            var result = nodes.Aggregate("", func);
            return result;
        }

        private static string GetString(string current, HtmlNode node) => current + "\r\n" + node.InnerText;

        [HttpGet("delete")]
        public IActionResult Delete(string site)
        {
            try
            {
                _rulesRepository.DeleteRule(site);
            }
            catch
            {
                return UnprocessableEntity();
            }

            return Ok();
        }
    }
}
