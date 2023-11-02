[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] != "perf") Examples.ProcessFiles(args);
        else Examples.RunPerformanceTests();
    }
}