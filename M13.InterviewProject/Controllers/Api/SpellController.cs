using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace M13.InterviewProject.Controllers.Api
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Services;

    [Route("api/spell")]
    public class SpellController : Controller
    {
        private readonly IHtmlReader _htmlReader;
        private readonly ISpellChecker _spellChecker;
        private readonly ILogger<SpellController> _logger;

        public SpellController(
            IHtmlReader htmlReader,
            ISpellChecker spellChecker,
            ILogger<SpellController> logger
        )
        {
            _htmlReader = htmlReader;
            _spellChecker = spellChecker;
            _logger = logger;
        }

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить список слов с ошибками
        /// </summary>
        [HttpGet("errors")]
        public async Task<IActionResult> Errors(string page)
        {
            try
            {
                var text = await _htmlReader.ReadPageAsync(page);

                var textErrors = new List<string>(100);

                var errors = await _spellChecker.GetErrors(text);

                textErrors.AddRange(errors.Select(error => error.Word));

                return Ok(textErrors);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        /// <summary>
        /// Проверить текст страницы по заданному адресу и получить количество слов с ошибками
        /// </summary>
        [HttpGet("errorscount")]
        public async Task<IActionResult> Count(string page)
        {
            try
            {
                var text = await _htmlReader.ReadPageAsync(page);
                _logger.LogDebug(text);
                var errors = await _spellChecker.GetErrors(text);
                return Ok(errors.Length);
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }
        }
    }
}
