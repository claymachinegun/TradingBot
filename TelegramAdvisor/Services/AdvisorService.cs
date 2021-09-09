using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TradingBot.Core;
namespace TelegramAdvisor.Services{
    public class AdvisorService {
        private IMemoryCache _cache;
        private ITradingApiProvider _apiProvider;
        private Advisor _advisor;
        private FixedRingBuffer<decimal> _prices;
        public AdvisorService(IMemoryCache cache, ITradingApiProvider apiProvider, Advisor advisor) {
            _advisor = advisor;
            _cache = cache;
            _apiProvider = apiProvider;
            _prices = new FixedRingBuffer<decimal>(60);
        }

        public async Task<string>  GetCurrentSignal(string coin, string fiat) {
            TradingSignal signal;
            if(!_cache.TryGetValue(coin+fiat, out signal)){
                var prices = await _apiProvider.GetPriceHistory(coin, fiat, 90);
                foreach(decimal price in prices){
                    _prices.Push(price);
                    _advisor.GetSignal(_prices);
                }
                var current = await _apiProvider.GetPairCurrentPrice(coin, fiat);
                _prices.Push(current);
                signal = _advisor.GetSignal(_prices);
                _cache.Set(coin+fiat, signal, TimeSpan.FromDays(1));
            }

            
            return signal.ToString();
        
        }

    }
}