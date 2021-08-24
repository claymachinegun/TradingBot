using NUnit.Framework;
using TradingBot.Core;
namespace TradingBotTests {
    
    public class AdvisorTests {
    private class FakeAlwaysSellIndicator : ITechIndicator {
        public TradingSignal GetSignal(FixedRingBuffer<decimal> price){
            return TradingSignal.SELL;
        }
    }


    private class FakeAlwaysBuyIndicator : ITechIndicator {
        public TradingSignal GetSignal(FixedRingBuffer<decimal> price){
            return TradingSignal.BUY;
        }
    }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAdvisorWeight() {
            Advisor advisor = new Advisor();
            advisor.Threshold = 0.5;
            advisor.AddIndicator(new FakeAlwaysBuyIndicator(), 0.6);
            advisor.AddIndicator(new FakeAlwaysSellIndicator(), 0.4);
            var signal = advisor.GetSignal(new FixedRingBuffer<decimal>(1));
            Assert.AreEqual(TradingSignal.BUY, signal);
        }

        [Test]
        public void TestAdvisorThreshold() {
            Advisor advisor = new Advisor();
            advisor.Threshold = 0.5;
            advisor.AddIndicator(new FakeAlwaysBuyIndicator(), 0.4);
            advisor.AddIndicator(new FakeAlwaysSellIndicator(), 0.4);
            var signal = advisor.GetSignal(new FixedRingBuffer<decimal>(1));
            Assert.AreEqual(TradingSignal.SELL, signal);
        }

    }
}