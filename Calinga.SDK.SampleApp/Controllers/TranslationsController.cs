using System.Threading.Tasks;
using Calinga.NET;
using Microsoft.AspNetCore.Mvc;

namespace Calinga.SDK.SampleApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslationsController : ControllerBase
    {
        private readonly ICalingaService _service;

        public TranslationsController(ICalingaService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<string> Get(string key, string language)
        {
            return _service.TranslateAsync(key, language);
        }
    }
}