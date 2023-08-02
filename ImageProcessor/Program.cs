[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = GetInputFilePathFromArgs(args);
        string outputFilePath = GetOutputFilePathFromArgs(args);

        Console.WriteLine($"Reading image from {inputFilePath}");
        Console.WriteLine("Processing image...");

        var imageReader = new ImageReader(inputFilePath);

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Invert);
        var invertFilterNode = new FilterNode(invertFilter);
        invertFilterNode.ConnectInput(imageReader);

        var mirrorFilter = FilterFactory.From(FilterFactory.Filter.Mirror);
        var mirrorFilterNode = new FilterNode(mirrorFilter);
        mirrorFilterNode.ConnectInput(invertFilterNode);

        var writer = new ImageWriter(outputFilePath);
        writer.ConnectInput(mirrorFilterNode);

        writer.Write();

        Console.WriteLine("Done!");
        Console.WriteLine("Output image saved to " + outputFilePath);
    }

    private static string GetInputFilePathFromArgs(string[] args)
    {
        return args.Length == 0 ? FindFirstImageInCurrentDirectory() : args[0];
    }

    private static string FindFirstImageInCurrentDirectory()
    {
        // Find the first image in the directory
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
        return args.Length < 2 ? "output.jpg" : args[1];
    }
}