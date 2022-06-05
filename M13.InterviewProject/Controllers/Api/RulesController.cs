namespace M13.InterviewProject.Controllers.Api
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Form;
    using Models.Implementation;
    using Repository;
    using Services;

    [Route("api/rules")]
    public class RulesController : Controller
    {
        private readonly IRulesRepository _rulesRepository;
        private readonly IHtmlReader _htmlReader;
        private readonly ILogger<RulesController> _logger;

        public RulesController(
            IRulesRepository rulesRepository,
            IHtmlReader htmlReader,
            ILogger<RulesController> logger
        )
        {
            _rulesRepository = rulesRepository;
            _htmlReader = htmlReader;
            _logger = logger;
        }

        [HttpPost("add")]
        public IActionResult AddRule([FromForm] AddRuleForm addRuleForm)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var newRule = new Rule(addRuleForm.Site, addRuleForm.Value);
            _logger.LogDebug(addRuleForm.ToString());
            _rulesRepository.AddRule(newRule);
            return Ok();
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
        public async Task<IActionResult> TestRules(string page)
        {
            try
            {
                var text = await _htmlReader.ReadPageAsync(page);
                return Ok(text);
            }
            catch (Exception e)
            {
                return UnprocessableEntity($"{e.Message}");
            }
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
