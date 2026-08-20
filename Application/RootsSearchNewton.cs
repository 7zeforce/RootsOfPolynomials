using Application;

namespace RootsSearchNewton
{
    public class RootsSearchNewton : RootsSearch
    {
        private const double Eps = 1e-9;
        private Func<double, double> _f;
        private int[] _intervals = new int[2];

        public RootsSearchNewton(Func<double, double> f, int[] intervals)
        {
            _f = f;
            _intervals = intervals;
        }

        public override List<double> FindAllRoots()
        {
            List<double> roots = new List<double>();
            List<double[]> intervals = SearchIntervals(roots, _intervals, _f,1, Eps);
            for (int i = 0; i < intervals.Count; i++)
            {
                double root = FindRoot(intervals[i][0]);
                if (Math.Abs(root - intervals[i][1]) > Eps) roots.Add(root);
                else throw new Exception("Newton do not work");
            }
            return roots.Select(r => r == 0 ? 0 : r).Distinct().OrderBy(r => r).ToList();
        }

        private double FindRoot(double startX, int maxIter = 100)
        {
            double xn = startX;
            for (int i = 0; i < maxIter; i++)
            {
                if (Math.Abs(_f(xn)) < Eps) return xn;
                double df = GetDerivative(xn);
                if (Math.Abs(df) > Eps) xn = xn - (_f(xn) / df);
                if (double.IsNaN(xn) || double.IsInfinity(xn)) throw new Exception("Newton method diverges");
            }
            throw new Exception("Newton do not work for this iterations");
        }

        private double GetDerivative(double point, double h = 1e-5)
        {
            return (_f(point+h) - _f(point-h))/(2*h);
        }
    }
}