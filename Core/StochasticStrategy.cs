using System;

public class StochasticStrategy
{
    public double Overbought = 80;
    public double Oversold = 20;
    public double StopLossPips = 50;
    public double TakeProfitPips = 100;
    public double RiskPercentPerTrade = 1.0;
    public bool UseVWAPFilter = true;
    public bool UseSessionFilter = true;
    public bool UseNewsFilter = true;

    public bool ShouldBuy(double slowKPrev, double dPrev, double slowK, double d, double price, double vwap, DateTime time, bool news)
    {
        if (slowKPrev >= dPrev || slowK <= d || slowK >= Oversold) return false;
        if (UseVWAPFilter && price < vwap) return false;
        if (UseSessionFilter && !IsTradingSession(time)) return false;
        if (UseNewsFilter && news) return false;
        return true;
    }

    public bool ShouldSell(double slowKPrev, double dPrev, double slowK, double d, double price, double vwap, DateTime time, bool news)
    {
        if (slowKPrev <= dPrev || slowK >= d || slowK <= Overbought) return false;
        if (UseVWAPFilter && price > vwap) return false;
        if (UseSessionFilter && !IsTradingSession(time)) return false;
        if (UseNewsFilter && news) return false;
        return true;
    }

    public bool IsTradingSession(DateTime time) => time.Hour >= 8 && time.Hour <= 17;

    public double CalculateLotSize(double equity, double stopLossPips, double pipValue)
    {
        double riskAmount = equity * RiskPercentPerTrade / 100.0;
        return riskAmount / (stopLossPips * pipValue);
    }

    public void LogTrade(string action, double price, double lots, double stopLoss, double takeProfit)
    {
        Console.WriteLine($"{DateTime.UtcNow}: {action} @ {price:F5} Lots={lots:F2} SL={stopLoss:F5} TP={takeProfit:F5}");
    }
}

