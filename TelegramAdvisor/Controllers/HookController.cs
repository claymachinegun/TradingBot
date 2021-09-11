using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramAdvisor.Services;

namespace TelegramAdvisor.Controllers{
    public class HookController : ControllerBase{
        
        AdvisorService _service;
        public HookController(AdvisorService service) {
            _service = service;
        }
        

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] TelegramReplyService handleUpdateService,
                                              [FromBody] Update update)

        {
            if(update != null) {
                await handleUpdateService.Reply(update);
                return Ok();
            }
            return BadRequest();
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