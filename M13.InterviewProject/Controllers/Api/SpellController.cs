using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace M13.InterviewProject.Controllers.Api
{
    using Services;
    using Services.Implementation;

    [Route("api/spell")]
    public class SpellController
    {
        private static readonly Dictionary<string, string> Rules = new();
        private readonly IHttpClientFactory _clientFactory;

        public SpellController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить список слов с ошибками
        /// </summary>
        [HttpGet("errors")]
        public IEnumerable<string> SpellErrors(
            string page
        )
        {
            var site = new Uri("http://" + page).Host;

            var text = _clientFactory.CreateClient().GetAsync("http://" + page)
                .ContinueWith(t =>
                {
                    var document = new HtmlDocument();
                    document.LoadHtml(t.Result.Content.ReadAsStringAsync().Result);
                    var xpath = Rules[site];
                    string innerText = "";
                    foreach (var node in document.DocumentNode.SelectNodes(xpath))
                    {
                        innerText = innerText + "\r\n" + node.InnerText;
                    }

                    return innerText;
                }).Result;

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
        [HttpGet("spell/errorscount")]
        public int SpellErrorsCount(
            string page
        )
        {
            var site = new Uri("http://" + page).Host;

            var text = _clientFactory.CreateClient().GetAsync("http://" + page)
                .ContinueWith(t =>
                {
                    var document = new HtmlDocument();
                    document.LoadHtml(t.Result.Content.ReadAsStringAsync().Result);
                    var xpath = Rules[site];
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
