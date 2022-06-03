using System.Collections.Generic;
using System.Linq;
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
        public async Task<int> Count(string page)
        {
            var text = await _htmlReader.ReadPageAsync(page);

            return new SpellChecker().GetErrors(text).Result.Count();
        }
    }
}
