using Application;

namespace RootsSearchBisection
{
    public sealed class RootsSearchBisection : RootsSearch
    {
        private const double Eps = 1e-9;

        private Func<double, double> _f;
        private int[] _intervals = new int[2];

        public RootsSearchBisection(Func<double, double> f, int[] intervals)
        {
            _f = f;
            _intervals = intervals;
        }

        public override List<double> FindAllRoots()
        {
            List<double> roots = new List<double>();
            List<double[]> intervals = SearchIntervals(roots, _intervals, _f,0.1,Eps);
            foreach (double[] inter in intervals)
            {
                double root = FindRoot(inter[0], inter[1]);
                if(Math.Abs(_f(root)) < 1e-6) roots.Add(double.Round(root, 5));
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
        
    }
}