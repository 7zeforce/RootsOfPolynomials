using Inpunter;
Console.WriteLine("Polynomial Root Finder!");
while (true)
{
    Console.Clear();
    Console.WriteLine("Polynomial Root Finder!");
    Inputer inputer = new Inputer();
    string input = inputer.Input();
    Func<double, double> f = inputer.CreateFunc();
    double[] Coificent = inputer.CreateCoificents();
    Console.WriteLine("Coificents:\n");
    foreach (double d in Coificent) Console.Write($"{d} ");
    Console.WriteLine("\nEnter the interval [a,b] when program will find roots:");
    int[] interval = { int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()) };
    RootsSearch.RootsSearch rootsSearch = new RootsSearch.RootsSearch(f, interval);
    List<double> roots = rootsSearch.FindAllRoots();
    if (roots.Count == 0) Console.WriteLine("This function do not have valid roots");
    else
    {
        Console.WriteLine("Roots found:");
        for (int i = 0; i < roots.Count; i++)
        {
            Console.WriteLine($"X_{i + 1} = {roots[i]}");
        }
    }
    Console.WriteLine("Press Escape to exit, or any other key to continue...");
    if (Console.ReadKey().Key == ConsoleKey.Escape)
        break;
}