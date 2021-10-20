using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Calinga.NET;

namespace Calinga.Demo.SampleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly ICalingaService _service;

        public DemoController(ICalingaService service)
        {
            _service = service;
        }

        [HttpGet("languages")]
        public Task<IEnumerable<string>> Get()
        {
            return _service.GetLanguagesAsync();
        }

        [HttpGet("referenceLanguage")]
        public Task<string> GetReferenceLanguage()
        {
            return _service.GetReferenceLanguage();
        }

        [HttpGet("translation")]
        public Task<string> Get(string key, string language)
        {
            return _service.TranslateAsync(key, language);
        }

        [HttpDelete("cache")]
        public void ClearCache()
        {
            _service.ClearCache();
        }
    }
}