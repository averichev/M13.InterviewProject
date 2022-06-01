using System;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace M13.InterviewProject.Controllers
{
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
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRulesRepository _rulesRepository;

        public ValuesController(IHttpClientFactory clientFactory, IRulesRepository rulesRepository)
        {
            _clientFactory = clientFactory;
            _rulesRepository = rulesRepository;
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
        public string TestRules(string page, string rule = null)
        {
            var site = new Uri("http://" + page).Host;

            var text = _clientFactory.CreateClient().GetAsync("http://" + page)
                .ContinueWith(t =>
                {
                    var document = new HtmlDocument();
                    document.LoadHtml(t.Result.Content.ReadAsStringAsync().Result);
                    string innerText = "";
                    foreach (var node in document.DocumentNode.SelectNodes(rule ?? _rulesRepository.GetRuleBySite(site)))
                    {
                        innerText = innerText + "\r\n" + node.InnerText;
                    }
                    return innerText;
                }).Result;
            return text;
        }

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
