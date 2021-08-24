using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TradingBot.Core;
using TradingBot.TechIndexes;
namespace TradingBotTests{
    public class RSISignalsTests {
        [SetUp]
        public void Setup()
        {
        }


        public List<decimal> FakeUpPriceFeed = new List<decimal>() {
            44.3389M,
            44.0902M,
            44.1497M,
            43.6124M,
            44.3278M,
            44.8264M,
            45.0955M,
            45.4245M,
            45.8433M,
            46.0826M,
            45.8931M,
            46.0328M,
            45.6140M,
            46.2820M,
            46.2820M
        };

        public List<decimal> FakePriceFeed = new List<decimal>() {
            46.4116M,
            46.2222M,
            45.6439M,
            46.2122M,
            46.2521M,
            45.7137M,
            46.4515M,
            45.7835M,
            45.3548M,
            44.0288M,
            44.1783M,
            44.2181M,
            44.5672M,
            43.4205M,
            42.6628M,
            43.1314M
        };

        [Test]
        public void RSIOverBuyedSellTest()
        {
            var priceFeed = new FixedRingBuffer<decimal>(15);
            var rsi = new RelativeStrengthIndex(14);
            var indicator = new TradingBot.TechIndicators.RSIOverSold(rsi);
            decimal? rsiValue = null; 
            foreach(var price in FakeUpPriceFeed) {
                priceFeed.Push(price);
                rsiValue = rsi.GetValue(priceFeed);
            }
            var signal = indicator.GetSignal(priceFeed);
            Assert.AreEqual(TradingSignal.SELL, signal);
        }

        [Test]
        public void RSIOverSoldTest(){
            var priceFeed = new FixedRingBuffer<decimal>(15);
            var rsi = new RelativeStrengthIndex(14);
            var indicator = new TradingBot.TechIndicators.RSIOverSold(rsi);
            decimal? rsiValue = null; 
            foreach(var price in FakeUpPriceFeed.OrderByDescending(x=>x)) {
                priceFeed.Push(price);
                rsiValue = rsi.GetValue(priceFeed);
            }
            var signal = indicator.GetSignal(priceFeed);
            Assert.AreEqual(TradingSignal.BUY, signal);
        }

        [Test]
        public void RSIAndPriceGainSellTest() {
            var priceFeed = new FixedRingBuffer<decimal>(15);
            var rsi = new RelativeStrengthIndex(14);
            var indicator = new TradingBot.TechIndicators.RSIAndPriceGain(rsi);
            decimal? rsiValue = null; 
            foreach(var price in FakeUpPriceFeed) {
                priceFeed.Push(price);
                rsiValue = rsi.GetValue(priceFeed);
            }
            priceFeed.Push(46.0028M);
            rsi.GetValue(priceFeed);
            var signal = indicator.GetSignal(priceFeed);
            Assert.AreEqual(TradingSignal.SELL, signal);
        }

        [Test]
        public void RSIAndPriceGainBuyTest(){
            var priceFeed = new FixedRingBuffer<decimal>(15);
            var rsi = new RelativeStrengthIndex(14);
            var indicator = new TradingBot.TechIndicators.RSIAndPriceGain(rsi);
            decimal? rsiValue = null; 
            foreach(var price in FakePriceFeed) {
                priceFeed.Push(price);
                rsiValue = rsi.GetValue(priceFeed);
            }
            var signal = indicator.GetSignal(priceFeed);
            Assert.AreEqual(TradingSignal.BUY, signal);
        }
    }
}