namespace M13.InterviewProject.Services.Implementation
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Models.Implementation;
    using Newtonsoft.Json;

    public class SpellChecker
    {
        private const string YandexSpellCheckerUrl = "http://speller.yandex.net/services/spellservice.json/checkText?text=";
        public async Task<ISpellCheckError[]> GetErrors(string text)
        {
            //используем сервис яндекса для поиска орфографических ошибок в тексте
            //сервис возвращает список слов, в которых допущены ошибки
            Task<HttpResponseMessage> task = new HttpClient()
                .GetAsync(YandexSpellCheckerUrl + WebUtility.UrlEncode(text));

            var errors = task.ContinueWith(t =>
            {
                int count = 0;
                var json = t.Result.Content.ReadAsStringAsync().Result;
                var errs = JsonConvert.DeserializeObject<SpellerErrors[]>(json);
                List<ISpellCheckError> list = new List<ISpellCheckError>(100);
                for (int i = 0; i < errs.Length; i++)
                {
                    list.Add(errs[i]);
                }
                return list;
            }).Result;

            return errors.ToArray();
        }
    }

}
