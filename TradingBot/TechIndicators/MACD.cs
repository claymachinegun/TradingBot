using TradingBot.TechIndexes;
using TradingBot.Core;
namespace TradingBot.TechIndicators
{
    public class MACD : ITechIndicator
    {
        private ExponentialMovingAverage _emaShort;
        private ExponentialMovingAverage _emaLong;
        private FixedRingBuffer<decimal> _macdBuffer;
        private ExponentialMovingAverage _emaSignal;

        public MACD(int shortEma, int longEma, int signal) {
            _emaShort = new ExponentialMovingAverage(shortEma);
            _emaLong = new ExponentialMovingAverage(longEma);
            _macdBuffer = new FixedRingBuffer<decimal>(signal + 1);
            _emaSignal = new ExponentialMovingAverage(signal);
        }

        public TradingSignal GetSignal(FixedRingBuffer<decimal> price)
        {
            decimal? shortvalue = _emaShort.GetValue(price);
            decimal? longvalue = _emaLong.GetValue(price);
            
            if(shortvalue == null || longvalue == null){
                return TradingSignal.WAIT;
            }

            _macdBuffer.Push(shortvalue.Value - longvalue.Value);

            decimal? signalValue = _emaSignal.GetValue(_macdBuffer);

            if(signalValue == null) {
                return TradingSignal.WAIT;
            } 

            if(_macdBuffer.GetLast() > signalValue && _macdBuffer.GetLast() > _macdBuffer.GetLastNth(1)) {
                return TradingSignal.BUY;
            } else {
                return TradingSignal.SELL;
            }

        }
    }
}