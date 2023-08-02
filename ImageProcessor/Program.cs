[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {

        Console.WriteLine("Processing image...");

        foreach (string arg in args) { }

        var imageReader = new ImageReader("./image.jpg");

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Invert);
        var invertFilterNode = new FilterNode(invertFilter);
        invertFilterNode.ConnectInput(imageReader);

        var mirrorFilter = FilterFactory.From(FilterFactory.Filter.Mirror);

        var mirrorFilterNode = new FilterNode(mirrorFilter);
        mirrorFilterNode.ConnectInput(invertFilterNode);

        var writer = new ImageWriter("./output.jpg");
        writer.ConnectInput(mirrorFilterNode);

        writer.Write();

        Console.WriteLine("Done!");
    }
}