using System;
using TradingBot.Binance;
using TradingBot.TechIndexes;
using TradingBot.Core;
using TradingBot.TechIndicators;
using System.Threading;
namespace TradingBotExample
{
    class Program
    {

        static int GetMillisecondsToNextTime(int intervalSeconds){
            long time = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            int seconds = intervalSeconds-1;
            int secondsLeft = (int)(seconds - (time % (intervalSeconds)));
            return secondsLeft * 1000;
        }
        static void Main(string[] args)
        {
                       
            var rsi = new RelativeStrengthIndex(14);
            var advisor = new Advisor();
            advisor.Threshold = 0.74;
            advisor.AddIndex(rsi);
            advisor.AddIndicator(new RSIAndPriceGain(rsi), 0.25);
            advisor.AddIndicator(new RSIOverSold(rsi), 0.25);
            advisor.AddIndicator(new MACross(6,12,12), 0.25);
            advisor.AddIndicator(new MACD(12,26,9), 0.25);



            var api = new BinanceApi(
                "<API_KEY>", 
                "<SIGNATURE>",
                "https://api.binance.com","1h");
            var bot = new TradingBot.Core.TradingBot(api, advisor, "BUSD", "ETH");
            bot.Initialize().Wait();
            Console.WriteLine("Initialized");
            while(true){
                Thread.Sleep(GetMillisecondsToNextTime(60*5));
                Console.Write(DateTime.Now.ToString());
                bot.TimerTick().Wait();
                Console.WriteLine();
            }
        }
    }
}
