using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace TradingBot.Core
{


    public class TradingBot
    {
        ITradingApiProvider _apiProvider;
        Advisor _advisor;
        private decimal _coinQuantity;
        private decimal _fiatQuantity;
        private string _fiat;
        private string _coin;
        private bool _refreshWallet;
        private int _fiatPrecision;
        private int _coinPrecision;
        private FixedRingBuffer<decimal> _prices;
        public TradingBot(ITradingApiProvider apiProvider, Advisor advisor, string fiat, string coin, int fiatPrecision = 4, int coinPrecision=6, int bufferSize = 152) {
            _advisor = advisor;
            _apiProvider = apiProvider;
            _fiat = fiat;
            _coin = coin;
            _prices = new FixedRingBuffer<decimal>(bufferSize);
            _refreshWallet = false;
            _fiatPrecision = fiatPrecision;
            _coinPrecision = coinPrecision;
        }


        public async Task UpdateWalletInfo() {
            var wallet = await _apiProvider.GetWalletInfo();
            if(!wallet.ContainsKey(_fiat) || !wallet.ContainsKey(_coin)){
                throw new Exception("Cannot get assets in wallet");
            }
            _fiatQuantity = wallet[_fiat];
            _coinQuantity = wallet[_coin];
            _refreshWallet = false;
        }

        public async Task Initialize(){
            await UpdateWalletInfo();
            await _apiProvider.CloseAllOpenOrders();
            var priceHistory = await _apiProvider.GetPriceHistory(_coin, _fiat, _prices.Buffer.Length);
            foreach(var outdatedPrice in priceHistory) {
                _prices.Push(outdatedPrice);
                _advisor.GetSignal(_prices);
            }
        }

        /// <summary>
        /// Check current price, get signal, place order
        /// </summary>
        /// <returns></returns>
        public async Task TimerTick() {
            var currentPrice = await _apiProvider.GetPairCurrentPrice(_coin, _fiat);
            if(_refreshWallet) {
                await UpdateWalletInfo();
            }
            _prices.Push(currentPrice);
            var signal = _advisor.GetSignal(_prices);
            switch(signal){
                case TradingSignal.BUY:
                case TradingSignal.HOLD:
                    if(_fiatQuantity > _coinQuantity*currentPrice) {
                        await _apiProvider.PlaceMarketBuyOrder(_coin, _fiat, precisionFloor(precisionFloor(_fiatQuantity, _fiatPrecision) / currentPrice, _coinPrecision));
                        _refreshWallet = true;
                    }
                    break;
                case TradingSignal.SELL:
                case TradingSignal.WAIT:
                    if(_coinQuantity*currentPrice > _fiatQuantity){
                        await _apiProvider.PlaceMarketSellOrder(_coin, _fiat, precisionFloor(_coinQuantity, _coinPrecision));
                        _refreshWallet = true;
                    }
                    break;
                default:
                    break;

            }
        }


        protected decimal precisionFloor(decimal value, int precision) {
            return Decimal.Round(value - 5M * (decimal)Math.Pow(10, -(precision+1)), precision);
        }
    }
}