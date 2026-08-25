
namespace Application
{
    internal sealed class Integrationer
    {
        public double Rectangles(Func<double, double> f, double a, double b, int n)
        {
            double dT = (b - a) / n;
            double area = 0;
            double x = a;
            for(int i = 1; i < n; i++)
            {
                if (!((double.IsNaN(f(x + (dT/2))) || double.IsInfinity(f(x + (dT / 2)))))){
                    area += f(x + (dT / 2)) * dT;
                    x += dT;
                }
            }
            return area;
        }

        public double Trapezoid(Func<double,double> f, double a, double b, int n)
        {
            double dT = (b - a) / n;
            double area = 0;
            double x = a;
            for(int i = 1; i < n; i++)
            {
                if (!((double.IsNaN(f(x)) || double.IsInfinity(f(x)) && (double.IsNaN(f(x + dT)) || double.IsInfinity(f(x + dT)))))){
                    area += ((f(x) + f(x + dT)) * dT) / 2;
                    x += dT;
                }
                else throw new Exception("The function diverges!");
            }
            return area;
        }

        public double Simpson(Func<double, double> f, double a, double b, int n)
        {
            double dT = (b - a) / n;
            double x = a + dT;
            double area = f(a)+f(b);
            for(int i = 1; i < n; i++)
            {
                area += (i % 2 == 0) ? 2 * f(x) : 4 * f(x);
                x += dT;
            }
            return area * (dT/3);
        }
    }
}
