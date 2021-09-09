using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegramAdvisor.Services;

namespace TelegramAdvisor.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class HookController : ControllerBase{
        
        AdvisorService _service;
        public HookController(AdvisorService service) {
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult> Get(string fiat, string coin){
            if(fiat == null || coin == null) {
                return BadRequest("fiat or coin parameter not specified");
            }
            var signal = await _service.GetCurrentSignal(coin, fiat);
            return Ok(signal);
        }
    }
}