using System.Collections.Generic;
using NUnit.Framework;
using TradingBot.Core;
namespace TradingBotTests{
    public class TradingBotTests {

        [SetUp]
        public void Setup()
        {
            
        }
    
        private Advisor getFakeAdvisor() {
            var advisor = new Advisor();
            advisor.Threshold = 0.1;
            advisor.AddIndicator(new FlipFlopFakeIndicator(), 1);
            return advisor;
        }


        [Test]
        public void TestPlacingOrder() {
            var fakeCryptoApi = new FakeCryptoApiProvider();
            var fakeAdvisor = getFakeAdvisor();
            var bot = new TradingBot.Core.TradingBot(fakeCryptoApi, getFakeAdvisor(), "USD", "BTC");
            bot.Initialize().Wait();
            Assert.AreEqual(false,fakeCryptoApi.IsBuyOrderPlaced);
            Assert.AreEqual(false,fakeCryptoApi.IsSellOrderPlaced);

            bot.TimerTick().Wait();
            Assert.AreEqual(true,fakeCryptoApi.IsBuyOrderPlaced);
            Assert.AreEqual(false,fakeCryptoApi.IsSellOrderPlaced);

            bot.TimerTick().Wait();
            Assert.AreEqual(false, fakeCryptoApi.IsSellOrderPlaced);
            Assert.AreEqual(true, fakeCryptoApi.IsBuyOrderPlaced);

            Assert.AreEqual(8.1,fakeCryptoApi.BuyQuantity);
        }
    }
}