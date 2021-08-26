using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingBot.Core {

    public interface ITradingApiProvider {
        Task<IDictionary<string,decimal>>  GetWalletInfo();
        Task<bool> CloseAllOpenOrders();
        Task<bool> CloseOpenedOrder(string orderId);
        Task<bool> IsOrderFilled(string orderId);

        Task PlaceMarketBuyOrder(string coin, string fiat, decimal quantity);
        Task PlaceMarketSellOrder(string coin, string fiat, decimal quantity);

        Task<decimal> GetPairCurrentPrice(string coin, string fiat);
        Task<IEnumerable<decimal>> GetPriceHistory(string coin, string fiat, int count);
    }
}