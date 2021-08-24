using System;
using System.Collections.Generic;

namespace TradingBot.Core
{
    public class Advisor {
        

        public double Threshold {get;set;}

        private List<WeightedIndicator> _indicators;
        private List<ITechIndex> _indexes;
        public Advisor() {
            _indicators = new List<WeightedIndicator>();
            _indexes = new List<ITechIndex>();
        }
        public void AddIndex(ITechIndex index) {
            _indexes.Add(index);
        }
        public void AddIndicator(ITechIndicator indicator, double weight) {
            _indicators.Add(new WeightedIndicator(indicator, weight));
        }

        private double TranslateSignal(TradingSignal signal) {
            switch(signal) {
                case TradingSignal.BUY:
                case TradingSignal.HOLD:
                    return 1;
                case TradingSignal.WAIT:
                case TradingSignal.SELL:
                default:
                    return 0;
            }
        }

        public TradingSignal GetSignal(FixedRingBuffer<decimal> price) {
            double value = 0;
            foreach(var index in _indexes) {
                index.GetValue(price);
            }
            foreach(var indicator in _indicators) {
                var val = TranslateSignal(indicator.Indicator.GetSignal(price)) * indicator.Weight;
                value += val;
            }
            if(value > Threshold ) {
                return TradingSignal.BUY;
            } else {
                return TradingSignal.SELL;
            }
        }
        
        
    }
}