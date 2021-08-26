using System.Collections.Generic;
using TradingBot.Core;
namespace TradingBotTests{
    public class FlipFlopFakeIndicator : ITechIndicator
    {
        private bool _nowSell  = false;

        public TradingSignal GetSignal(FixedRingBuffer<decimal> price)
        {
            _nowSell = !_nowSell;
            return _nowSell ? TradingSignal.BUY : TradingSignal.SELL;
        }
    }
}