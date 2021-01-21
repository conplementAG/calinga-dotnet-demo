using System.Collections.Generic;
using System.Threading.Tasks;
using Calinga.NET;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("translation")]
        public Task<string> Get(string key, string language)
        {
            return _service.TranslateAsync(key, language);
        }
    }
}