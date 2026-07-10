using Inpunter;
using System.Numerics;

Console.WriteLine("Polynomial Root Finder!");
while (true)
{
    Console.Clear();
    Console.WriteLine("Polynomial Root Finder!");
    Inputer inputer = new Inputer();
    inputer.Input();
    Func<double, double> f = inputer.CreateFunc();

    double[]? coefficients = null;
    try
    {
        coefficients = inputer.CreateCoificents();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Coificents are not available: {ex.Message}");
    }

    if (coefficients != null)
    {
        Console.Write("Coificents: ");
        foreach (double d in coefficients) Console.Write($"{d} ");
        Console.WriteLine();

        var solver = new ComplexRootsSearch.ComplexRootsSearch(coefficients);
        List<Complex> roots = solver.RootsSearch();
        PrintComplexRoots(roots);
    }
    else
    {
        Console.WriteLine("Enter the interval [a,b] where the program will find roots:");
        int[] interval = { int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()) };
        RootsSearch.RootsSearch rootsSearch = new RootsSearch.RootsSearch(f, interval);
        List<double> realRoots = rootsSearch.FindAllRoots();
        if (realRoots.Count == 0) Console.WriteLine("This function do not have valid real roots");
        else
        {
            Console.WriteLine("Real roots found:");
            for (int i = 0; i < realRoots.Count; i++)
                Console.WriteLine($"X_{i + 1} = {realRoots[i]}");
        }
    }

    Console.WriteLine("Press Escape to exit, or any other key to continue...");
    if (Console.ReadKey().Key == ConsoleKey.Escape)
        break;
}

static void PrintComplexRoots(List<Complex> roots)
{
    if (roots.Count == 0)
    {
        Console.WriteLine("This function do not have roots");
        return;
    }
    Console.WriteLine("Roots found:");
    int i = 1;
    foreach (Complex z in roots.OrderBy(r => r.Real).ThenBy(r => r.Imaginary))
    {
        double re = Math.Round(z.Real, 5);
        double im = Math.Round(z.Imaginary, 5);
        if (Math.Abs(im) < 1e-6)
            Console.WriteLine($"X_{i} = {re}");
        else
            Console.WriteLine($"X_{i} = {re} {(im >= 0 ? "+" : "-")} {Math.Abs(im)}i");
        i++;
    }
}
