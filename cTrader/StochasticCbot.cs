
using cAlgo.API;

namespace cAlgo
{
    [Robot(TimeZone=TimeZones.UTC, AccessRights=AccessRights.None)]
    public class StochasticCbot : Robot
    {
        [Parameter("K Period", DefaultValue = 14)]
        public int KPeriod { get; set; }

        [Parameter("K Smoothing", DefaultValue = 3)]
        public int KSmoothing { get; set; }

        [Parameter("D Period", DefaultValue = 3)]
        public int DPeriod { get; set; }

        [Parameter("Volume", DefaultValue = 10000)]
        public int Volume { get; set; }

        [Parameter("StopLoss Pips", DefaultValue = 50)]
        public int StopLoss { get; set; }

        [Parameter("TakeProfit Pips", DefaultValue = 100)]
        public int TakeProfit { get; set; }

        private StochasticOscillator _stoch;
        private IndicatorDataSeries _vwap;
        private StochasticStrategy Strategy = new StochasticStrategy();

        protected override void OnStart()
        {
            _stoch = Indicators.Stochastic(KPeriod, KSmoothing, DPeriod, MovingAverageType.Simple);
            _vwap = Indicators.VWAP().VWAP;
        }

        protected override void OnBar()
        {
            double slowK = _stoch.PercentK.Last(0);
            double d = _stoch.PercentD.Last(0);
            double slowKPrev = _stoch.PercentK.Last(1);
            double dPrev = _stoch.PercentD.Last(1);
            double price = Bars.ClosePrices.Last(0);
            double vwap = _vwap.Last(0);
            bool news = false;

            if(Strategy.ShouldBuy(slowKPrev,dPrev,slowK,d,price,vwap,Server.Time,news) && Positions.Count==0)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, Volume, StopLoss, TakeProfit);
                Strategy.LogTrade("BUY", price, Volume, price-StopLoss*Symbol.PipSize, price+TakeProfit*Symbol.PipSize);
            }

            if(Strategy.ShouldSell(slowKPrev,dPrev,slowK,d,price,vwap,Server.Time,news) && Positions.Count==0)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, Volume, StopLoss, TakeProfit);
                Strategy.LogTrade("SELL", price, Volume, price+StopLoss*Symbol.PipSize, price-TakeProfit*Symbol.PipSize);
            }
        }
    }
}
