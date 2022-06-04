using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace M13.InterviewProject.Controllers.Api
{
    using System.Threading.Tasks;
    using Services;

    [Route("api/spell")]
    public class SpellController
    {
        private readonly IHtmlReader _htmlReader;
        private readonly ISpellChecker _spellChecker;

        public SpellController(
            IHtmlReader htmlReader,
            ISpellChecker spellChecker
        )
        {
            _htmlReader = htmlReader;
            _spellChecker = spellChecker;
        }

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить список слов с ошибками
        /// </summary>
        [HttpGet("errors")]
        public async Task<IEnumerable<string>> Errors(string page)
        {
            var text = await _htmlReader.ReadPageAsync(page);

            var textErrors = new List<string>(100);

            var errors = await _spellChecker.GetErrors(text);

            textErrors.AddRange(errors.Select(error => error.Word));

            return textErrors;
        }

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить количество слов с ошибками
        /// </summary>
        [HttpGet("errorscount")]
        public async Task<int> Count(string page)
        {
            var text = await _htmlReader.ReadPageAsync(page);

            var errors = await _spellChecker.GetErrors(text);
            return errors.Length;
        }
    }
}
