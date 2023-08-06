using ImageProcessorNS;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{

    private static readonly FileLogger logger = new();

    public static void Main(string[] args)
    {
        string inputFilePath = GetInputFilePathFromArgs(args);
        string outputFilePath = GetOutputFilePathFromArgs(args);

        logger.WriteLine($"\nReading image from {inputFilePath}");
        logger.WriteLine("Processing image...");

        var imageReader = new ImageReader(inputFilePath);

        var statsNode = Stats(imageReader);
        var outputNode = EdgeDetection(statsNode);
        var writer = new ImageWriter(outputFilePath);

        writer.ConnectInput(outputNode);

        var elapsedMilliseconds = writer.Write();

        logger.WriteLine("Done! Image processed in " + elapsedMilliseconds + "ms.");
        logger.WriteLine("Output image saved to " + outputFilePath + '\n');
    }

    private static FilterNode EdgeDetection(FilterNode.IReadable reader)
    {
        var blackAndWhiteFilterNode = FilterFactory.FilterNodeFrom(FilterFactory.Filter.Black_And_White);
        blackAndWhiteFilterNode.ConnectInput(reader);

        var sobelFilterNode = FilterFactory.FilterNodeFrom(FilterFactory.Filter.Sobel);
        sobelFilterNode.ConnectInput(blackAndWhiteFilterNode);

        return sobelFilterNode;
    }

    private static FilterNode InvertMirror(FilterNode.IReadable reader)
    {
        var invertFilterNode = FilterFactory.FilterNodeFrom(FilterFactory.Filter.Invert);
        invertFilterNode.ConnectInput(reader);

        var mirrorFilterNode = FilterFactory.FilterNodeFrom(FilterFactory.Filter.Mirror);
        mirrorFilterNode.ConnectInput(invertFilterNode);

        return mirrorFilterNode;
    }

    private static FilterNode Stats(FilterNode.IReadable reader)
    {
        var stats = new ImageStats(new FileLogger());
        var filterNode = new FilterNode(stats);
        filterNode.ConnectInput(reader);

        return filterNode;
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