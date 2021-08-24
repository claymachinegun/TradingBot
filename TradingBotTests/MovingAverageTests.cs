using NUnit.Framework;
using TradingBot.Core;
using TradingBot.TechIndexes;
namespace TradingBotTests{
    public class MovingAverageTests {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MovingAverageCalculationTest()
        {
            MovingAverage ma = new MovingAverage(3,3);
            var buffer = new FixedRingBuffer<decimal>(3);
            buffer.Push(2);
            buffer.Push(3);
            buffer.Push(7);
            var value = ma.GetValue(buffer);
            Assert.AreEqual(4, value);
            buffer.Push(5);
            value = ma.GetValue(buffer);
            Assert.AreEqual(5,value);
        }
    }
}