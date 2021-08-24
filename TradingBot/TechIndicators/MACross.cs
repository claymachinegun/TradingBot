using TradingBot.Core;
using TradingBot.TechIndexes;
namespace TradingBot.TechIndicators
{
    public class MACross : ITechIndicator
    {
        private MovingAverage _signal;
        private MovingAverage _trend;


        public MACross(int signal, int trend, int size) {
            _signal = new MovingAverage(signal, signal + 1);
            _trend = new MovingAverage(trend, trend + 1 );
        }
        public TradingSignal GetSignal(FixedRingBuffer<decimal> price)
        {
           decimal? signal = _signal.GetValue(price);
           decimal? trend = _trend.GetValue(price);
           
           if(signal == null || trend == null) {
               return TradingSignal.WAIT;
           }

           TradingSignal value = signal > trend && _signal.Buffer.GetLastNth(1) < signal ? TradingSignal.BUY : TradingSignal.SELL; 
           return value;
        }
    }
}