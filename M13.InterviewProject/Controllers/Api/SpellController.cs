using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace M13.InterviewProject.Controllers.Api
{
    using System.Threading.Tasks;
    using Repository;
    using Services;
    using Services.Implementation;

    [Route("api/spell")]
    public class SpellController
    {
        private readonly IRulesRepository _rulesRepository;
        private readonly IHtmlReader _htmlReader;
        private readonly IHttpClientFactory _clientFactory;

        public SpellController(
            IHttpClientFactory clientFactory,
            IRulesRepository rulesRepository,
            IHtmlReader htmlReader
        )
        {
            _clientFactory = clientFactory;
            _rulesRepository = rulesRepository;
            _htmlReader = htmlReader;
        }

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить список слов с ошибками
        /// </summary>
        [HttpGet("errors")]
        public async Task<IEnumerable<string>> Errors(string page)
        {
            var text = await _htmlReader.ReadPageAsync(page);

            var textErrors = new List<string>(100);

            new SpellChecker().GetErrors(text).ContinueWith(r =>
            {
                for (int i = 0; i < r.Result.Length; i++)
                {
                    textErrors.Add(r.Result[i].Word);
                }
            });

            return textErrors;
        }

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить количество слов с ошибками
        /// </summary>
        [HttpGet("errorscount")]
        public int Count(string page)
        {
            var site = new Uri("http://" + page).Host;

            var text = _clientFactory.CreateClient().GetAsync("http://" + page)
                .ContinueWith(t =>
                {
                    var document = new HtmlDocument();
                    document.LoadHtml(t.Result.Content.ReadAsStringAsync().Result);
                    var xpath = _rulesRepository.GetRuleBySite(site);
                    string innerText = "";
                    foreach (var node in document.DocumentNode.SelectNodes(xpath))
                    {
                        innerText = innerText + "\r\n" + node.InnerText;
                    }

                    return innerText;
                }).Result;

            return new SpellChecker().GetErrors(text).Result.Count();
        }
    }
}
