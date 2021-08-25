using System.Collections.Generic;
using NUnit.Framework;
using TradingBot.Core;
using TradingBot.TechIndexes;
namespace TradingBotTests{
    public class ExponentialMovingAverageTests {
        [SetUp]
        public void Setup()
        {
        }

        private List<decimal> _fakePriceFeed = new List<decimal>(){
          14M,13M,14M,12M,13M,12M,11M  
        };

        [Test]
        public void ExponentialMovingAverageCalculationTest()
        {
            var ema = new ExponentialMovingAverage(5);
            var priceFeed = new FixedRingBuffer<decimal>(5);
            decimal? value = null;
            foreach(var currentValue in _fakePriceFeed){
                priceFeed.Push(currentValue);
                value = ema.GetValue(priceFeed);
            }
            Assert.NotNull(value);
            Assert.AreEqual(12.2M, value);
            
        }
    }
}