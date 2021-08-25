using System.Collections.Generic;
using NUnit.Framework;
using TradingBot.Core;
using TradingBot.TechIndicators;
namespace TradingBotTests{
    public class MACDTests {
        [SetUp]
        public void Setup()
        {
        }

        private List<decimal> _fakePriceFeed = new List<decimal> {
            459.99M,
            448.85M,
            446.06M,
            450.81M,
            442.8M,
            448.97M,
            444.57M,
            441.4M,
            430.47M,
            420.05M,
            431.14M,
            425.66M,
            430.58M,
            431.72M,
            437.87M,
            428.43M,
            428.35M,
            432.5M,
            443.66M,
            455.72M,
            454.49M,
            452.08M,
            452.73M,
            461.91M,
            463.58M,
            461.14M,
            452.08M,
            442.66M,
            428.91M,
            429.79M,
            431.99M,
            427.72M,
            423.2M,
            426.21M,
            426.98M
        };

        [Test]
        public void MACDSignalSellTest()
        {
            var macd = new MACD(12,26,9);
            var signal = TradingSignal.WAIT;
            var buffer = new FixedRingBuffer<decimal>(50);
            foreach(var price in _fakePriceFeed) {
                buffer.Push(price);
                signal = macd.GetSignal(buffer);
            }
            Assert.AreEqual(TradingSignal.SELL, signal);
        }
    }
}