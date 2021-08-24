using TradingBot.TechIndexes;
using TradingBot.Core;
using System.Linq;
namespace TradingBot.TechIndicators
{
    public class RSIAndPriceGain : ITechIndicator
    {
        private RelativeStrengthIndex _rsi;
        public RSIAndPriceGain(RelativeStrengthIndex index) {
            _rsi = index;
        }

        public TradingSignal GetSignal(FixedRingBuffer<decimal> price)
        {
            if(_rsi.Buffer.Count < 2 || _rsi.Buffer.GetLast() == null || _rsi.Buffer.GetLastNth(1) == null) {
                return TradingSignal.WAIT;
            }
            decimal? rsiValue = _rsi.Buffer.GetLast();
            decimal? rsiOldvalue = _rsi.Buffer.GetLastNth(1);
            
            decimal currentPrice = price.GetLast();
            decimal lastPrice = price.GetLastNth(1);

            TradingSignal signal = TradingSignal.WAIT;

            if(rsiValue > rsiOldvalue && currentPrice > lastPrice) {
                signal = TradingSignal.BUY;
            } else  {
                signal = TradingSignal.SELL;
            }
            return signal;
        }
    }
}