using System.Collections.Generic;
using System.Threading.Tasks;
using TradingBot.Core;
namespace TradingBotTests{
    public class FakeCryptoApiProvider : ITradingApiProvider
    {
        private bool _isBuyOrderPlaced = false;
        private bool _isSellOrderPlaced = false;
        private decimal _sellQuantity = 0;
        private decimal _buyQuantity = 0;

        public decimal BuyQuantity => _buyQuantity;
        public decimal SellQuantity => _sellQuantity;
        public bool IsBuyOrderPlaced => _isBuyOrderPlaced;
        public bool IsSellOrderPlaced => _isSellOrderPlaced;
        public Task<bool> CloseAllOpenOrders()
        {
            return Task.Run<bool>(()=> true);
        }

        public Task<bool> CloseOpenedOrder(string orderId)
        {
            return Task.Run<bool>(()=> true);
        }

        public Task<decimal> GetPairCurrentPrice(string coin, string fiat)
        {
            return Task.Run<decimal>(()=> 1.23456789M);
        }

        public Task<IEnumerable<decimal>> GetPriceHistory(string coin, string fiat, int count)
        {
            return Task.Run<IEnumerable<decimal>>(()=> new List<decimal>());
        }

        public Task<IDictionary<string, decimal>> GetWalletInfo()
        {
            return Task.Run<IDictionary<string, decimal>>(()=>{
                Dictionary<string, decimal> result = new Dictionary<string, decimal>();
                result.Add("USD", 10);
                result.Add("BTC", 0);
                return result;
            });
        }

        public Task<bool> IsOrderFilled(string orderId)
        {
            return Task.Run<bool>(()=> true);
        }

        public Task PlaceMarketBuyOrder(string coin, string fiat, decimal quantity)
        {
            _isBuyOrderPlaced = true;
            _isSellOrderPlaced = false;
            _buyQuantity = quantity;
            _sellQuantity = 0;
            return Task.Run(()=> {});
        }

        public Task PlaceMarketSellOrder(string coin, string fiat, decimal quantity)
        {
            _isBuyOrderPlaced = false;
            _isSellOrderPlaced = true;
            _sellQuantity = quantity;
            _buyQuantity = 0;
            return Task.Run(()=> {});
        }
    }
}