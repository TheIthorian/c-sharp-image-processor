using System.Drawing.Imaging;
using CommandLine;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            RunNoInput();
            Console.WriteLine("Please press any enter to finish...");
            Console.ReadLine();
            return;
        }

        try
        {

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);
        }
        catch (Exception error)
        {
            Console.WriteLine("Error!");
            Console.WriteLine(error.Message);

            Console.WriteLine("Please press any enter to finish...");
            Console.ReadLine();
        }
    }

    private static void RunNoInput()
    {
        var inputFile = "";
        while (inputFile == "")
        {
            Console.WriteLine("Location of input file: ");
            inputFile = Console.ReadLine();
        }

        Console.WriteLine("\nLocation of output file (output.png): ");
        var outputFile = Console.ReadLine();

        Console.WriteLine("\nWidth in px (leave blank to keep aspect ratio): ");
        var width = Console.ReadLine() ?? "0";
        var intWidth = Int32.Parse(width == "" ? "0" : width);

        Console.WriteLine("\nHeight in px (leave blank to keep aspect ratio): ");
        var height = Console.ReadLine() ?? "0";
        var intHeight = Int32.Parse(height == "" ? "0" : height);

        Console.WriteLine("\nResizing " + inputFile);
        var inputReader = new ImageReader(inputFile);
        var resizer = new ResizeImage(inputReader.Read());
        var outputImage = resizer.Resize(intWidth, intHeight);

        outputImage.Save(outputFile ?? "output.png", ImageFormat.Png);
    }

    private static void RunOptions(CommandLineOptions opts)
    {
        if (opts.Performance) Examples.RunPerformanceTests();
        else if (opts.Resize) ResizeImage(opts);
    }

    private static void ResizeImage(CommandLineOptions opts)
    {
        if (opts.InputImage == null) throw new Exception("Please provide an input image using --inputImage");

        Console.WriteLine("Resizing " + opts.InputImage);

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