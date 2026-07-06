using Inpunter;
Console.WriteLine("Polynomial Root Finder!");
while (true)
{
    Console.Clear();
    Console.WriteLine("Polynomial Root Finder!");

    Inputer inputer = new Inputer();
    string input = inputer.Input();
    Func<double, double> f = inputer.CreateFunc(input);
    RootsSearch.RootsSearch rootsSearch = new RootsSearch.RootsSearch(f);

    Console.WriteLine("Enter count of iterations:");
    int iterations = int.Parse(Console.ReadLine());

    List<double> roots = rootsSearch.FindAllRoots(new double[] { 10 }, iterations);
    Console.WriteLine("Roots found:");
    for (int i = 0; i < roots.Count; i++)
    {
        Console.WriteLine($"X_{i + 1} = {roots[i]}");
    }

    Console.WriteLine("Press Escape to exit, or any other key to continue...");
    if (Console.ReadKey().Key == ConsoleKey.Escape)
        break;
}