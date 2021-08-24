namespace TradingBot.Core
{
    public interface ITechIndicator
    {
         TradingSignal GetSignal(FixedRingBuffer<decimal> price);
    }
}