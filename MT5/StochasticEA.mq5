#property strict

input int KPeriod = 14;
input int DPeriod = 3;
input int Slowing = 3;
input double LotSize = 0.1;
input double StopLossPips = 50;
input double TakeProfitPips = 100;

CTrade trade;
int stochHandle;

bool CheckNews(){ return false; }
double VWAP(){ return iCustom(_Symbol,PERIOD_CURRENT,"VWAP",0,0); }

int OnInit()
{
    stochHandle = iStochastic(_Symbol,PERIOD_CURRENT,KPeriod,Slowing,DPeriod,MODE_SMA);
    return(INIT_SUCCEEDED);
}

void OnTick()
{
    double k[2], d[2];
    if(CopyBuffer(stochHandle,0,0,2,k)<=0) return;
    if(CopyBuffer(stochHandle,1,0,2,d)<=0) return;

    double price = SymbolInfoDouble(_Symbol,SYMBOL_ASK);
    double vwap = VWAP();
    bool news = CheckNews();

    if(k[1]<d[1] && k[0]>d[0] && k[0]<20 && price>vwap && PositionsTotal()==0)
        trade.Buy(LotSize,_Symbol,price,price-StopLossPips*Point,price+TakeProfitPips*Point);

    if(k[1]>d[1] && k[0]<d[0] && k[0]>80 && price<vwap && PositionsTotal()==0)
        trade.Sell(LotSize,_Symbol,price,price+StopLossPips*Point,price-TakeProfitPips*Point);
}

