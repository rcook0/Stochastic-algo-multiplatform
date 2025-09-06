#property strict

input int KPeriod = 14;
input int DPeriod = 3;
input int Slowing = 3;
input double LotSize = 0.1;
input double StopLossPips = 50;
input double TakeProfitPips = 100;

int ticket;
bool CheckNews() { return false; } // placeholder
double VWAP() { return iCustom(NULL,0,"VWAP",0,0); } // must have VWAP indicator

void OnTick()
{
    double k     = iStochastic(NULL,0,KPeriod,Slowing,DPeriod,MODE_SMA,0,MODE_MAIN,0);
    double d     = iStochastic(NULL,0,KPeriod,Slowing,DPeriod,MODE_SMA,0,MODE_SIGNAL,0);
    double kPrev = iStochastic(NULL,0,KPeriod,Slowing,DPeriod,MODE_SMA,0,MODE_MAIN,1);
    double dPrev = iStochastic(NULL,0,KPeriod,Slowing,DPeriod,MODE_SMA,0,MODE_SIGNAL,1);
    double price = Ask;
    double vwap  = VWAP();
    bool news    = CheckNews();

    if(kPrev < dPrev && k > d && k < 20 && price>vwap && OrdersTotal()==0)
    {
        ticket = OrderSend(Symbol(),OP_BUY,LotSize,Ask,3,Ask-StopLossPips*Point,Ask+TakeProfitPips*Point,"Buy",0,0,clrBlue);
        Print("BUY executed at ", Ask);
    }

    if(kPrev > dPrev && k < d && k > 80 && price<vwap && OrdersTotal()==0)
    {
        ticket = OrderSend(Symbol(),OP_SELL,LotSize,Bid,3,Bid+StopLossPips*Point,Bid-TakeProfitPips*Point,"Sell",0,0,clrRed);
        Print("SELL executed at ", Bid);
    }
}

