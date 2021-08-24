using TradingBot.TechIndexes;
using TradingBot.Core;
using System.Linq;
namespace TradingBot.TechIndicators
{
    public class RSIOverSold : ITechIndicator
    {
        private RelativeStrengthIndex _rsi;
        private TradingSignal _lastSignal;
        public RSIOverSold(RelativeStrengthIndex index) {
            _rsi = index;
            _lastSignal = TradingSignal.WAIT;
        }
        public TradingSignal GetSignal(FixedRingBuffer<decimal> price)
        {
            //decimal rsiValue = _rsi.GetValue(price);
            if(_rsi.Buffer.Count == 0 || (_rsi.Buffer.GetLast() == null)) {
                return TradingSignal.WAIT;
            }
            decimal? rsiValue = _rsi.Buffer.GetLast();
            TradingSignal signal = TradingSignal.WAIT;

            if(rsiValue < 31) {
                signal = TradingSignal.BUY;
            } else if (rsiValue > 69) {
                signal = TradingSignal.SELL;
            } else if(_lastSignal != TradingSignal.WAIT) {
                signal = _lastSignal;
            } else {
                signal = TradingSignal.WAIT;
            }
            _lastSignal = signal;
            return signal;
            
        }
    }
}