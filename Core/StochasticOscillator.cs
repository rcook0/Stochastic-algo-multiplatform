using System;

public static class StochasticOscillator
{
    public static (double[] fastK, double[] slowK, double[] d) Compute(
        double[] highs, double[] lows, double[] closes,
        int fastKPeriod = 14, int smoothK = 3, int dPeriod = 3)
    {
        int n = closes.Length;
        var fastK = new double[n];
        var slowK = new double[n];
        var d = new double[n];

        for (int i = 0; i < n; i++)
        {
            if (i < fastKPeriod - 1)
            {
                fastK[i] = double.NaN;
                continue;
            }

            double highestHigh = double.MinValue;
            double lowestLow = double.MaxValue;

            for (int j = i - fastKPeriod + 1; j <= i; j++)
            {
                if (highs[j] > highestHigh) highestHigh = highs[j];
                if (lows[j] < lowestLow) lowestLow = lows[j];
            }

            double range = highestHigh - lowestLow;
            fastK[i] = range <= 0.0 ? (i > 0 ? fastK[i - 1] : 50.0) : 100.0 * (closes[i] - lowestLow) / range;
        }

        SMAInto(fastK, smoothK, slowK);
        SMAInto(slowK, dPeriod, d);

        return (fastK, slowK, d);
    }

    private static void SMAInto(double[] src, int period, double[] dst)
    {
        int n = src.Length;
        for (int i = 0; i < n; i++)
        {
            if (i < period - 1 || double.IsNaN(src[i]))
            {
                dst[i] = double.NaN;
                continue;
            }

            double sum = 0.0;
            for (int j = i - period + 1; j <= i; j++)
            {
                if (double.IsNaN(src[j]))
                {
                    sum = double.NaN;
                    break;
                }
                sum += src[j];
            }

            dst[i] = double.IsNaN(sum) ? double.NaN : sum / period;
        }
    }
}

