namespace TradingBot.Core {
internal class WeightedIndicator {
            public WeightedIndicator(ITechIndicator indicator, double weight) 
            {
                Indicator = indicator;
                Weight = weight;
            }

            public ITechIndicator Indicator {get;set;}
            public double Weight {get; set;}
        }
}