namespace TradingBot.Binance
{
    public class CoinInfo {
        public string Coin {get; set;}
        public decimal Free {get; set;}
        public decimal Locked {get; set;}
        public string Name {get; set;}
        public decimal Storage {get ; set;}
        public bool Trading {get; set;}
    }        
}