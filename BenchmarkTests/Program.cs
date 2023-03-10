using BenchmarkDotNet.Running;

namespace BenchmarkTests;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Initializing benchmarks");
        BenchmarkRunner.Run<BenchmarkDataAccess>();
        BenchmarkRunner.Run<BenchmarkPrioritizationModel>();
    }
}
