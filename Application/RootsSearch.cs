using System.Drawing;

namespace RootsSearch
{
    public class RootsSearch
    {
        private Func<double, double> _f;
        
        public RootsSearch(Func<double, double> f)
        {
            _f = f;
        }

        public List<double> FindAllRoots(double[] initialPoints, int maxIterations, double tolerance = 1e-6)
        {
            HashSet<double> uniqueRoots = new HashSet<double>();
            foreach (var start in initialPoints)
            {
                double point = start;
                for (int i = 0; i < maxIterations; i++)
                {
                    double value = _f(point);
                    if (Math.Abs(value) < tolerance)
                    {
                        double root = Math.Round(point, 10);
                        uniqueRoots.Add(root);
                        break;
                    }
                    double derivative = NumericalDerivative(point);
                    if (Math.Abs(derivative) < 1e-12) break;
                    point = point - value / derivative;
                }
            }
            return uniqueRoots.ToList();
        }

        private double NumericalDerivative(double x, double h = 1e-5)
        {
            return (_f(x + h) - _f(x - h)) / (2 * h);
        }
    }
}