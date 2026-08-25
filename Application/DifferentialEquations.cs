using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public sealed class DifferentialEquations
    {
        public double EulerFunc(double point, Func<double,double,double> f, double t0, double y0, double h=1e-1)
        {
            double y = y0;
            double t = t0;
            int step = (int)((1 / h) * point);
            for (int i = 0; i < step; i++)
            {
                y = y + h * f(t,y);
                t += h;
            }
            return y;
        }

        public double StaticEulerFunc(double point, Func<double, double, double> f, double t0, double y0, double h = 1e-1)
        {
            double y = y0;
            double t = t0;
            y = y + h * f(t, y);
            int step = (int)((1 / h) * point);
            for (int i = 0; i < step-1; i++)
            {
                t += h;
                y = y + h * f(t, y);
            }
            return y;
        }
    }
}
