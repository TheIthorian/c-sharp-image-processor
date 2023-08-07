using ImageProcessorNS;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{

    private static readonly FileLogger logger = new();

    public static void Main(string[] args)
    {
        if (args.Length == 0 || args[0] != "perf")
        {
            ProcessFiles(args);
            return;
        }

        RunPerformanceTests();
    }

    private static void RunPerformanceTests()
    {
        var tester = new PerformanceTester(logger);
        tester.Test();
    }

    private static void ProcessFiles(string[] args)
    {
        string inputFilePath = GetInputFilePathFromArgs(args);
        string outputFilePath = GetOutputFilePathFromArgs(args);

        logger.WriteLine($"\nReading image from {inputFilePath}");
        logger.WriteLine("Processing image...");

        var imageReader = new ImageReader(inputFilePath);

        var statsNode = FilterFactory.Stats(imageReader);
        var outputNode = FilterFactory.EdgeDetection(statsNode);
        var writer = new ImageWriter(outputFilePath);

        writer.ConnectInput(outputNode);

        var elapsedMilliseconds = writer.Write();

        logger.WriteLine("Done! Image processed in " + elapsedMilliseconds + "ms.");
        logger.WriteLine("Output image saved to " + outputFilePath + '\n');
    }

    private static string GetInputFilePathFromArgs(string[] args)
    {
        return args.Length == 0 ? FindFirstImageInCurrentDirectory() : args[0];
    }

    private static string FindFirstImageInCurrentDirectory()
    {
        var files = Directory.GetFiles(Directory.GetCurrentDirectory());
        var inputFilePath = files.FirstOrDefault(file => file.EndsWith(".jpg") || file.EndsWith(".png"));

        if (inputFilePath == null)
        {
            throw new AppExceptions.NoInputConnected("Unable to find input image in current directory.");
        }

        return inputFilePath;
    }

    private static string GetOutputFilePathFromArgs(string[] args)
    {
        return args.Length < 2 ? Path.Combine(Directory.GetCurrentDirectory(), "output.jpg") : args[1];
    }
}