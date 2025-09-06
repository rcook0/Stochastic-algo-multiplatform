
using System;

class Program
{
    static void Main()
    {
        double[] highs = {/* your OHLC data */};
        double[] lows = {/* your OHLC data */};
        double[] closes = {/* your OHLC data */};
        double[] vwapArr = {/* VWAP array */};
        bool[] newsArr = new bool[closes.Length];
        DateTime[] times = new DateTime[closes.Length];
        for(int i=0;i<times.Length;i++) times[i] = DateTime.UtcNow.AddMinutes(i*5);

        var Strategy = new StochasticStrategy();
        double balance = 10000;
        double pipValue = 0.1;

        var (fastK, slowK, d) = StochasticOscillator.Compute(highs, lows, closes);

        for(int i=1;i<closes.Length;i++)
        {
            double price = closes[i];
            double vwap = vwapArr[i];
            bool news = newsArr[i];

            if(Strategy.ShouldBuy(slowK[i-1], d[i-1], slowK[i], d[i], price, vwap, times[i], news))
            {
                double lots = Strategy.CalculateLotSize(balance, Strategy.StopLossPips, pipValue);
                Strategy.LogTrade("BUY", price, lots, price-Strategy.StopLossPips*pipValue, price+Strategy.TakeProfitPips*pipValue);
            }

            if(Strategy.ShouldSell(slowK[i-1], d[i-1], slowK[i], d[i], price, vwap, times[i], news))
            {
                double lots = Strategy.CalculateLotSize(balance, Strategy.StopLossPips, pipValue);
                Strategy.LogTrade("SELL", price, lots, price+Strategy.StopLossPips*pipValue, price-Strategy.TakeProfitPips*pipValue);
            }
        }
    }
}
