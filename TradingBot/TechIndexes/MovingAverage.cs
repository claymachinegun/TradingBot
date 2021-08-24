using System.Linq;
using TradingBot.Core;

namespace TradingBot.TechIndexes
{
    public class MovingAverage : ITechIndex
    {
        private int _count;
        private FixedRingBuffer<decimal> _buffer;

        public FixedRingBuffer<decimal> Buffer => _buffer;
        public MovingAverage(int count, int bufferSize) {
            _count = count;
            _buffer = new FixedRingBuffer<decimal>(bufferSize);
        }

        public decimal? GetValue(FixedRingBuffer<decimal> values) {
            if(values.Count < _count) {
                return null;
            }
            decimal value = values.Buffer.Take(values.Count).TakeLast(_count).Average();
            _buffer.Push(value);
            return value;


        }

    }
}