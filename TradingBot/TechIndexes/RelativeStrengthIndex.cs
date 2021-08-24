using System.Linq;
using TradingBot.Core;
namespace TradingBot.TechIndexes
{
    public class RelativeStrengthIndex : ITechIndex
    {
        private FixedRingBuffer<decimal?> _priceGain;
        private FixedRingBuffer<decimal?> _priceLoss;
        private FixedRingBuffer<decimal?> _buffer;
        private int _count;


        public FixedRingBuffer<decimal?> Buffer => _buffer;

        public RelativeStrengthIndex(int count) {
            _count = count;
            _priceGain = new FixedRingBuffer<decimal?>(count);
            _priceLoss = new FixedRingBuffer<decimal?>(count);
            _buffer = new FixedRingBuffer<decimal?>(count);
        }

        public decimal? GetValue(FixedRingBuffer<decimal> values)
        {
            if(values.Count < 2) {
                return null;
            }
            decimal? priceChange = values.GetLast() - values.GetLastNth(1);
            _priceGain.Push(priceChange > 0 ? priceChange : 0);
            _priceLoss.Push(priceChange <= 0 ? -priceChange : 0 );

            if(_priceGain.Count < _count) {
                return null;
            }
            decimal? averageGain = _priceGain.Buffer.Take(_priceGain.Count).TakeLast(_count).Average();
            decimal? averageLoss = _priceLoss.Buffer.Take(_priceLoss.Count).TakeLast(_count).Average();
            decimal? rsiValue = 0;
            if(averageLoss == 0 ){
                rsiValue = 100;
            } else {
                rsiValue = 100 - (100/ (1 + (averageGain / averageLoss)));
            }
            _buffer.Push(rsiValue);
            return rsiValue;
        }
    }
}