using System.Drawing.Imaging;
using CommandLine;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(RunOptions)
            .WithNotParsed(HandleParseError);
    }

    private static void RunOptions(CommandLineOptions opts)
    {
        if (opts.Performance) Examples.RunPerformanceTests();
        else if (opts.Resize) ResizeImage(opts);
    }

    private static void ResizeImage(CommandLineOptions opts)
    {
        if (opts.InputImage == null) throw new Exception("Please provide an input image using --inputImage");

        var inputReader = new ImageReader(opts.InputImage);
        var resizer = new ResizeImage(inputReader.Read());
        var outputImage = resizer.Resize(opts.Width, opts.Height);

        outputImage.Save(opts.OutputImage ?? "output.png", ImageFormat.Png);
    }

    private static void HandleParseError(IEnumerable<Error> errs)
    {
        Console.WriteLine("There are " + errs.Count() + " errors");
        Console.WriteLine(errs.First());
    }
}