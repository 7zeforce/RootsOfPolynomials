using System.Numerics;

namespace ComplexRootsSearch
{
    public class ComplexRootsSearch
    {
        private readonly double[] _coefficients;

        public ComplexRootsSearch(double[] coefficients)
        {
            _coefficients = coefficients;
        }

        public List<Complex> RootsSearch(int maxIter = 100)
        {
            int deg = _coefficients.Length - 1;
            if (deg < 1) return new List<Complex>();

            Complex[] points = CreatePoints();
            for (int iter = 0; iter < maxIter; iter++)
            {
                double maxDelta = 0;
                for (int i = 0; i < points.Length; i++)
                {
                    Complex den = Complex.One;
                    for (int j = 0; j < points.Length; j++)
                        if (i != j) den *= points[i] - points[j];

                    Complex delta = P(points[i]) / den;
                    points[i] -= delta;
                    maxDelta = Math.Max(maxDelta, Complex.Abs(delta));
                }
                if (maxDelta < 1e-12) break;
            }
            return points.ToList();
        }

        private Complex P(Complex z)
        {
            int deg = _coefficients.Length - 1;
            double lead = _coefficients[deg];
            Complex result = Complex.One;
            for (int i = deg - 1; i >= 0; i--)
                result = result * z + _coefficients[i] / lead;
            return result;
        }

        private Complex[] CreatePoints()
        {
            int deg = _coefficients.Length - 1;
            double lead = _coefficients[deg];
            Complex centre = -_coefficients[deg - 1] / (deg * (Complex)lead);
            double radius = 1 + _coefficients.Take(deg).Max(c => Math.Abs(c)) / Math.Abs(lead);

            Complex[] points = new Complex[deg];
            for (int i = 0; i < deg; i++)
                points[i] = centre + radius * Complex.Exp(Complex.ImaginaryOne * (2 * Math.PI * i / deg + 0.5));
            return points;
        }
    }
}
