# StochasticFramework

A unified Stochastic Oscillator trading strategy implemented across multiple platforms:

- **MT4 EA** (`MT4/StochasticEA.mq4`)  
- **MT5 EA** (`MT5/StochasticEA.mq5`)  
- **cTrader cBot** (`cTrader/StochasticCbot.cs`)  
- **Pure C# Backtester** (`Backtester/Program.cs`)  

Includes:

- Pure C# Stochastic oscillator calculation
- VWAP filter
- Session & news filters
- Position sizing based on risk
- SL/TP management
- Trade logging

---

## Usage

### MT4 / MT5
1. Copy the `.mq4` or `.mq5` file to your `Experts` folder.  
2. Open MetaEditor → Compile.  
3. Attach EA to a chart.  
4. Adjust input parameters as needed (StopLoss, TakeProfit, LotSize, Filters).  

### cTrader
1. Copy `.cs` cBot file to cTrader → Automate.  
2. Compile → Run on chart.  
3. Configure parameters in the cBot panel.

### C# Backtester
1. Open `Backtester/StochasticFramework.csproj` in **Visual Studio 2022**.  
2. Ensure **.NET 6.0 SDK** is installed.  
3. Replace sample OHLC / VWAP / news arrays in `Program.cs` with your real data.  
4. Build → Run → Review console logs for trade simulation.  

---

## Example Usage

### MT4 / MT5
BUY executed at 1.25678 Lots=0.50 SL=1.25178 TP=1.26678

SELL executed at 1.26432 Lots=0.50 SL=1.26932 TP=1.25432

*(Logged in MetaTrader Experts tab)*

### C# Backtester Output
2025-08-28 11:05: BUY @ 1.25678 Lots=0.50 SL=1.25178 TP=1.26678

2025-08-28 11:20: SELL @ 1.26432 Lots=0.50 SL=1.26932 TP=1.25432

## Parameters

| Platform | Key Parameters                     |
|----------|-----------------------------------|
| All      | Overbought, Oversold, StopLoss, TakeProfit, LotSize/Risk% |
| Filters  | VWAP Filter, Session Filter, News Filter (enable/disable) |

---

## Notes
- SL/TP and lot sizing configurable per platform.  
- Filters can be toggled via `StochasticStrategy.cs`.  
- Designed to **produce consistent signals** across MT4, MT5, cTrader, and C# backtester.

---

## License
MIT License

