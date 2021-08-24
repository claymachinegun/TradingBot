using NUnit.Framework;
using TradingBot.Core;
using TradingBot.TechIndicators;
namespace TradingBotTests{
    public class MovingAveragesCrossTests {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MovingAverageCrossCorrectnessTest()
        {
            var mACross = new MACross(2,4,4);
            var fakePriceFeed = new FixedRingBuffer<decimal>(4);
            fakePriceFeed.Push(3);
            fakePriceFeed.Push(2);
            fakePriceFeed.Push(1);
            fakePriceFeed.Push(2);
            var sellSignal = mACross.GetSignal(fakePriceFeed);
            Assert.AreEqual(TradingSignal.SELL, sellSignal);
            fakePriceFeed.Push(2);
            fakePriceFeed.Push(3);
            var buySignal = mACross.GetSignal(fakePriceFeed);
            Assert.AreEqual(TradingSignal.BUY, buySignal);
        }
    }
}