using System.Linq;
using TradingBot.Core;
namespace TradingBot.TechIndexes
{
    public class ExponentialMovingAverage : ITechIndex
    {
        private int _count;
        private decimal _weightCoefficient;

        private decimal? _lastValue;
        public ExponentialMovingAverage(int count) {
            _lastValue = null;
            _count = count;
            _weightCoefficient = 2.0M / (count + 1);
        }

        public decimal? GetValue(FixedRingBuffer<decimal> values)
        {
            if(values.Count < _count) {
                return null;
            }
            decimal? value = 0;
            if(_lastValue == null) {
                value = values.Buffer.Take(values.Count).TakeLast(_count).Average();
            } else {
                value = values.GetLast() * _weightCoefficient + (1-_weightCoefficient) * _lastValue;
            }
            _lastValue = value;
            return value;

        }
    }
}
