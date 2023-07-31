Console.WriteLine("Hello, World!");

foreach (string arg in args)
{
    Console.WriteLine(arg);
}
// ExampleProcessor.Run();

var imageReader = new ImageReader("../image.jpg");

var invertFilter = FilterFactory.FromString("InvertFilter");
var invertFilterNode = new FilterNode(invertFilter);
invertFilterNode.ConnectInput(imageReader);

var writer = new ImageWriter("../output.jpg");
writer.ConnectInput(invertFilterNode);

writer.Write();