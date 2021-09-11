using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramAdvisor.Services{
    public class TelegramReplyService {
        ITelegramBotClient _bot;
        public TelegramReplyService(ITelegramBotClient bot) {
            _bot = bot;
        }
        public async Task Reply(Update update) {
            await _bot.SendTextMessageAsync(update.Message.Chat.Id, "WAAAAAGH");
        }
    }
}