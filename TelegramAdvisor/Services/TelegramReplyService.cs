using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TradingBot.Core;

namespace TelegramAdvisor.Services{
    public class TelegramReplyService {
        private ITelegramBotClient _bot;
        private AdvisorService _advisorService;
        
        private string[] fiats = new string[]{"USDT", "EUR" ,"RUB"};
        private string[] coins = new string[]{"ETH", "BTC", "ADA", "XRP", "NANO"};

        private IEnumerable<IEnumerable<InlineKeyboardButton>> GetKeyboard(string[] list, string prefix = "") {
            var result = new List<List<InlineKeyboardButton>>();

            List<InlineKeyboardButton> row = null;
            for(int i = 0; i < list.Length; i++) {
                if(row == null) {
                    row = new List<InlineKeyboardButton>();
                    result.Add(row);
                }
                InlineKeyboardButton button = new InlineKeyboardButton();
                if(string.IsNullOrEmpty(prefix)){
                    button.Text = list[i];
                    button.CallbackData =list[i];
                } else {
                    button.Text = prefix + "/" + list[i];
                    button.CallbackData = prefix + ":" + list[i];
                }
                row.Add(button);
                if((i+1) % 3 == 0) {
                    row = null;
                }
            }
            return result;
        }

        public TelegramReplyService(ITelegramBotClient bot, AdvisorService advisor) {
            _bot = bot;
            _advisorService = advisor;
        }

        public async Task Reply(Update update) {
            InlineKeyboardMarkup markup = null;
            long chatId = 0;
            if(string.IsNullOrEmpty(update.CallbackQuery?.Data)) {
                chatId = update.Message.Chat.Id;
                var keys = GetKeyboard(fiats);
                markup = new InlineKeyboardMarkup(keys);
                await _bot.SendTextMessageAsync(chatId, "Choose fiat", replyMarkup: markup);
            } else {
                chatId = update.CallbackQuery.Message.Chat.Id;
                if(update.CallbackQuery.Data.Contains(':')){
                    var pair = update.CallbackQuery.Data.Split(':');
                    await _bot.SendTextMessageAsync(chatId, await _advisorService.GetCurrentSignal(pair[1], pair[0]));
                } else {
                    markup = new InlineKeyboardMarkup(GetKeyboard(coins, update.CallbackQuery.Data));
                    await _bot.SendTextMessageAsync(chatId, "Choose coin", replyMarkup: markup);
                }
            }
            
        }
    }
}