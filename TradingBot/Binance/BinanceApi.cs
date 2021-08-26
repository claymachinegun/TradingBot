using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

using System.Security.Cryptography;
using System.Text;
using System.Linq;
using TradingBot.Core;

namespace TradingBot.Binance {
    public class BinanceApi : ITradingApiProvider{
        public string ApiKey {get; set;}
        private HMACSHA256 _sign;
        public string BaseUri {get; set;}
        private RestClient _client;

        private DateTime _localStartTime;
        private long _serverStartTime;
        private string _timePeriod;

        public BinanceApi(string apiKey, string sercurityKey, string baseUri, string timePeriod) {
            _client = new RestClient(baseUri);
            _client.AddDefaultHeader("X-MBX-APIKEY", apiKey);
            _timePeriod = timePeriod;
            _sign = new HMACSHA256(Encoding.UTF8.GetBytes(sercurityKey));
            updateServerTime();
        }

        protected string GetSignature(string query) {
            StringBuilder resultBuilder = new StringBuilder();
            var bytes = _sign.ComputeHash(Encoding.UTF8.GetBytes(query));
            foreach(var chunk in bytes) {
                resultBuilder.Append(chunk.ToString("x2"));
            }
            return resultBuilder.ToString();
        }

        protected string AddSignature(string query) {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.Append(query);
            resultBuilder.Append("&signature=");
            var bytes = _sign.ComputeHash(Encoding.UTF8.GetBytes(query));
            foreach(var chunk in bytes) {
                resultBuilder.Append(chunk.ToString("x2"));
            }
            return resultBuilder.ToString();
        }

        public async Task<long> GetServerTime() {
            RestRequest request = new RestRequest("api/v3/time");
            var result = await _client.GetAsync<ServerTimeResponse>(request);
            if(result == null) return -1; 
            return result.ServerTime;
        }


        private void updateServerTime(){
            _serverStartTime = GetServerTime().GetAwaiter().GetResult();
            _localStartTime = DateTime.Now;

            if(_serverStartTime == 0) {
                throw new Exception("Cannot get correct Server Time");
            }
        }
        private long getTimestamp(){
            long _localDelta = (long)(DateTime.Now - _localStartTime).TotalMilliseconds;
            if(_localDelta > 1000*60) {
                updateServerTime();
                _localDelta = (long)(DateTime.Now - _localStartTime).TotalMilliseconds;
            }
            return _serverStartTime + _localDelta;
        }


        public async Task<decimal> GetPairCurrentPrice(string coin, string fiat)
        {
            RestRequest request = new RestRequest($"/api/v3/ticker/price?symbol={coin+fiat}");
            var result = await _client.GetAsync<CurrentPriceResponse>(request);
            if(result != null) {
                return result.Price;
            }
            return -1;
        }

        public async Task<IDictionary<string, decimal>> GetWalletInfo()
        {
            long timestamp = getTimestamp();
            string requestParameters = AddSignature($"timestamp={timestamp}");
            RestRequest request = new RestRequest($"sapi/v1/capital/config/getall?{requestParameters}");
            var result = await _client.GetAsync<List<CoinInfo>>(request);

            Dictionary<string, decimal> formatted = new Dictionary<string, decimal>();
            foreach(CoinInfo info in result) {
                formatted.Add(info.Coin, info.Free);
            }
            return formatted;
        }

        public async Task<bool> CloseAllOpenOrders()
        {
            return await Task.Run(()=> false);
        }

        public Task<bool> CloseOpenedOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<decimal>> GetPriceHistory(string coin, string fiat, int count) {
            RestRequest request = new RestRequest($"api/v3/klines?symbol={coin+fiat}&interval={_timePeriod}&limit={count}");
            var result = await _client.GetAsync<List<List<Object>>>(request);
            if(result != null) {
                List<decimal> prices = new List<decimal>();
                foreach(var kline in result) {
                    decimal price = 0;
                    if(decimal.TryParse(kline[4].ToString(), out price)){
                        prices.Add(price);
                    }
                }
                prices.RemoveAt(prices.Count-1);
                return prices;
            }
            return null;
        }

        public Task<bool> IsOrderFilled(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task PlaceMarketBuyOrder(string coin, string fiat, decimal quantity)
        {
            long timestamp = getTimestamp();
            string requestParameters = AddSignature($"symbol={coin+fiat}&side=BUY&type=MARKET&quantity={quantity}&timestamp={timestamp}&recvWindow=10000");
            RestRequest request = new RestRequest($"api/v3/order?{requestParameters}");
            var result = await _client.PostAsync<string>(request);
            Console.WriteLine(result.ToString());
        }

        public async Task PlaceMarketSellOrder(string coin, string fiat, decimal quantity)
        {
            long timestamp = getTimestamp();
            string requestParameters = AddSignature($"symbol={coin+fiat}&side=SELL&type=MARKET&quantity={quantity}&timestamp={timestamp}&recvWindow=10000");
            RestRequest request = new RestRequest($"api/v3/order?{requestParameters}");
            var result = await _client.PostAsync<string>(request);
            Console.WriteLine(result.ToString());
        }

    }
}