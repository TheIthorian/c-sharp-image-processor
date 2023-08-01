[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class Program
{
    public static void Main(string[] args)
    {

        Console.WriteLine("Processing image...");

        foreach (string arg in args) { }

        var imageReader = new ImageReader("../image.jpg");

        var invertFilter = FilterFactory.FromString("InvertFilter");
        var invertFilterNode = new FilterNode(invertFilter);
        invertFilterNode.ConnectInput(imageReader);

        var writer = new ImageWriter("../output.jpg");
        writer.ConnectInput(invertFilterNode);

        writer.Write();

        Console.WriteLine("Done!");
    }
}