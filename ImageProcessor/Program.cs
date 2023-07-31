Console.WriteLine("Processing image...");

foreach (string arg in args)
{
}
// ExampleProcessor.Run();

var imageReader = new ImageReader("../image.jpg");

var invertFilter = FilterFactory.FromString("InvertFilter");
var invertFilterNode = new FilterNode(invertFilter);
invertFilterNode.ConnectInput(imageReader);

var writer = new ImageWriter("../output.jpg");
writer.ConnectInput(invertFilterNode);

writer.Write();

Console.WriteLine("Done!");
