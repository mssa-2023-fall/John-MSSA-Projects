
Console.WriteLine(Fib(5));
Console.WriteLine(Factorial(5));

foreach(var item in FibWithoutRecursion(100))
{
    Console.WriteLine(item);
    if (item < 0) break;
}

foreach (var item in FibWithoutRecursionAndYieldReturn(100))
{
    Console.WriteLine(item);
    if (item < 0) break;
}

static int Fib (int input)
{
    if(input == 0) return 0;
    if(input == 1) return 1;

    return Fib(input - 1) + Fib(input - 2);
}

static int Factorial(int input)
{
    if (input == 0) return 1;
    return input * Factorial(input - 1);
}

static int[] FibWithoutRecursion(int input)
{
    int[] fibSeries = new int[input + 1];
    fibSeries[0] = 0;
    fibSeries[1] = 1;

    for(int i = 2; i <=input; i++)
    {
        fibSeries[i] = fibSeries[i - 1] + fibSeries[i - 2];
    }
    Console.WriteLine("Entire resultset are computed");
    return fibSeries;
}

static IEnumerable<int> FibWithoutRecursionAndYieldReturn(int input)
{
    int[] fibSeries = new int[input + 1];
    fibSeries[0] = 0;
    fibSeries[1] = 1;
    for(int i = 2; i <= input; i++)
    {
        fibSeries[i] = fibSeries[i - 1] + fibSeries[i - 2];
        yield return fibSeries[i];
    }
}
