namespace M13.InterviewProject.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Services;
    using System.Threading.Tasks;
    using Models;
    using Models.Implementation;
    using Newtonsoft.Json;

    public class SpellChecker : ISpellChecker
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SpellChecker(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        private const string YandexSpellCheckerUrl =
            "https://speller.yandex.net/services/spellservice.json/checkText?text=";

        public async Task<ISpellCheckError[]> GetErrors(string text)
        {
            //используем сервис яндекса для поиска орфографических ошибок в тексте
            //сервис возвращает список слов, в которых допущены ошибки
            var response = await _httpClientFactory.CreateClient()
                .GetAsync(YandexSpellCheckerUrl + WebUtility.UrlEncode(text));

            var json = await response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<SpellerErrors[]>(json);
            var list = new List<ISpellCheckError>(100);
            list.AddRange((errors ?? Array.Empty<SpellerErrors>()).Cast<ISpellCheckError>());
            return list.ToArray();
        }
    }
}
