using System.Collections.Generic;
using NUnit.Framework;
using TradingBot.Core;
using TradingBot.TechIndexes;
namespace TradingBotTests{
    public class RSITests {
        [SetUp]
        public void Setup()
        {
        }


        public IEnumerable<decimal> GetFakePriceFeed(){
            yield return 44.3389M;
            yield return 44.0902M;
            yield return 44.1497M;
            yield return 43.6124M;
            yield return 44.3278M;
            yield return 44.8264M;
            yield return 45.0955M;
            yield return 45.4245M;
            yield return 45.8433M;
            yield return 46.0826M;
            yield return 45.8931M;
            yield return 46.0328M;
            yield return 45.6140M;
            yield return 46.2820M;
            yield return 46.2820M;
            yield break;
        } 

        [Test]
        public void RSICalculationCorrectnessTests()
        {
            var priceFeed = new FixedRingBuffer<decimal>(15);
            var rsi = new RelativeStrengthIndex(14);
            decimal? rsiValue = null; 
            foreach(var price in GetFakePriceFeed()) {
                priceFeed.Push(price);
                rsiValue = rsi.GetValue(priceFeed);
            }
            Assert.NotNull(rsiValue);
            Assert.AreEqual(70.53M, decimal.Round(rsiValue.Value, 2));
        }
    }
}