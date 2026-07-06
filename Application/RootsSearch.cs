using System.Drawing;
using System.Runtime.Intrinsics.X86;

namespace RootsSearch
{
    public class RootsSearch
    {
        private const double Eps = 1e-9;

        private Func<double, double> _f;
        private int[] _intervals = new int[2];

        public RootsSearch(Func<double, double> f, int[] intervals)
        {
            _f = f;
            _intervals = intervals;
        }

        public List<double> FindAllRoots()
        {
            List<double> roots = new List<double>();
            List<double[]> intervals = SearchIntervals(roots);
            foreach (double[] inter in intervals)
            {
                roots.Add(double.Round(FindRoot(inter[0], inter[1]), 5));
            }
            return roots.Select(r => r == 0 ? 0 : r).Distinct().OrderBy(r => r).ToList();
        }

        private double FindRoot(double a, double b, double tolerance = 1e-10, int maxIter = 200)
        {
            double fa = _f(a);
            for (int iter = 0; iter < maxIter && (b - a) > tolerance; iter++)
            {
                double m = (a + b) / 2;
                double fm = _f(m);
                if (fm == 0) return m;
                if (Math.Sign(fm) == Math.Sign(fa)) { a = m; fa = fm; }
                else b = m;
            }
            return (a + b) / 2;
        }

        private List<double[]> SearchIntervals(List<double> exactRoots)
        {
            List<double[]> intervals = new List<double[]>();
            double step = 0.1;
            double prev = _intervals[0];
            bool havePrev = TryEvaluate(prev, out double fPrev);
            if (havePrev && Math.Abs(fPrev) < Eps) exactRoots.Add(double.Round(prev, 5));
            for (double x = _intervals[0] + step; x <= _intervals[1] + 1e-9; x += step)
            {
                if (x > _intervals[1]) x = _intervals[1];
                if (!TryEvaluate(x, out double fx))
                {
                    prev = x; havePrev = false;
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
            }
            return intervals;
        }

        private bool TryEvaluate(double x, out double value)
        {
            try
            {
                value = _f(x);
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