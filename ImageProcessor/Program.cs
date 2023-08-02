[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = GetInputFilePathFromArgs(args);
        string outputFilePath = GetOutputFilePathFromArgs(args);

        Console.WriteLine($"\nReading image from {inputFilePath}");
        Console.WriteLine("Processing image...");

        var imageReader = new ImageReader(inputFilePath);

        var invertFilterNode = FilterFactory.FilterNodeFrom(FilterFactory.Filter.Invert);
        invertFilterNode.ConnectInput(imageReader);

        var mirrorFilterNode = FilterFactory.FilterNodeFrom(FilterFactory.Filter.Mirror);
        mirrorFilterNode.ConnectInput(invertFilterNode);

        var writer = new ImageWriter(outputFilePath);
        writer.ConnectInput(mirrorFilterNode);

        var elapsedMilliseconds = writer.Write();

        Console.WriteLine("Done! Image processed in " + elapsedMilliseconds + "ms.");
        Console.WriteLine("Output image saved to " + outputFilePath + '\n');
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