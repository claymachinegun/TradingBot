namespace TradingBot.Core
{
    public interface ITechIndex
    {
        decimal? GetValue(FixedRingBuffer<decimal> values);
    }
}