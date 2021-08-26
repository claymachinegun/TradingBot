using System.Collections.Generic;

namespace TradingBot.Binance
{

    public class BalanceSnapshotData {
        public List<BalanceInfo> Balances {get; set;}
        public decimal TotalAssetOfBtc { get; set;}
    }
    public class BalanceSnapshot {
        public BalanceSnapshotData Data {get;set;}
        public string Type {get; set;}
        public long Updatetime {get; set;}
    }
    public class BalanceInfo {
        public string Asset {get; set;}
        public decimal Free {get; set;}
        public decimal Locked {get; set;}
    }
    public class AccountInfoResponse {
        public int Code {get; set;}
        public string Msg {get; set;}
        public List<BalanceSnapshot> SnapshotVos {get; set;}
        
    }
}