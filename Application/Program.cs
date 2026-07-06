using System.Globalization;
using Inpunter;
Inputer inputer = new Inputer();
string input = inputer.Input();
Func<double, double> f = inputer.CreateFunc(input);
Console.WriteLine("Enter a value for x:");
double x = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
double result = f(x);
Console.WriteLine(result);