using CommandLine;

public class CommandLineOptions
{
    [Option('p', "performance", Default = false, HelpText = "Performance test mode")]
    public bool Performance { get; set; }

    [Option('r', "resize", Default = true, HelpText = "Resize image mode")]
    public bool Resize { get; set; }

    [Option('i', "inputImage", HelpText = "Image source")]
    public string? InputImage { get; set; }

    [Option('o', "outputImage", HelpText = "Image output")]
    public string? OutputImage { get; set; }

    [Option('W', "width", HelpText = "Width of the resized image")]
    public int? Width { get; set; }

    [Option('H', "height", HelpText = "Height of the resized image")]
    public int? Height { get; set; }
}