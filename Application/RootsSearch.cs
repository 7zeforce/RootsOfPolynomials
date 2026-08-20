
namespace Application
{
    public abstract class RootsSearch
    {
        public abstract List<double> FindAllRoots();

        public virtual List<double[]> SearchIntervals(List<double> exactRoots, int[] _intervals, Func<double,double> f,double step = 0.1,double Eps = 1e-9)
        {
            List<double[]> intervals = new List<double[]>();
            double prev = _intervals[0];
            bool havePrev = TryEvaluate(prev,f, out double fPrev);
            if (havePrev && Math.Abs(fPrev) < Eps) exactRoots.Add(double.Round(prev, 5));
            for (double x = _intervals[0] + step; x <= _intervals[1] + 1e-9; x += step)
            {
                if (x > _intervals[1]) x = _intervals[1];
                if (!TryEvaluate(x, f, out double fx))
                {
                    prev = x; havePrev = false;
                    if (x == _intervals[1]) break;
                    continue;
                }

                if (Math.Abs(fx) < Eps)
                {
                    exactRoots.Add(double.Round(x, 5));
                }
                else if (havePrev && Math.Abs(fPrev) >= Eps && Math.Sign(fx) != Math.Sign(fPrev))
                {
                    intervals.Add(new double[2] { prev, x });
                }
                prev = x; fPrev = fx; havePrev = true;
                if (x == _intervals[1]) break;
            }
            return intervals;
        }

        public virtual bool TryEvaluate(double x, Func<double, double> f, out double value)
        {
            try
            {
                value = f(x);
                return !double.IsNaN(value) && !double.IsInfinity(value);
            }
            catch
            {
                value = 0;
                return false;
            }
        }
    }
}
