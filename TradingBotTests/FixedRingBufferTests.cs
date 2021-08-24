using System;
using NUnit.Framework;
using TradingBot.Core;
namespace TradingBotTests
{
    public class FixedRingBufferTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FixedRingBufferShift()
        {
            FixedRingBuffer<int> buffer = new FixedRingBuffer<int>(3);
            Assert.AreEqual(0,buffer.Count);
            buffer.Push(1);
            Assert.AreEqual(1,buffer.Count);
            for(int i = 0; i < 10; i++){
                buffer.Push(i);
            }
            Assert.AreEqual(3,buffer.Count);
            
        }
        [Test]
        public void FixedRingBufferLastValues() {
            FixedRingBuffer<int> buffer = new FixedRingBuffer<int>(3);
            for(int i = 0; i < 10; i++){
                buffer.Push(i);
            }
            Assert.AreEqual(9,buffer.GetLast());
            Assert.AreEqual(7,buffer.GetLastNth(2));
            Assert.Throws<IndexOutOfRangeException>(delegate {
                buffer.GetLastNth(5);
            });
        }
    }
}