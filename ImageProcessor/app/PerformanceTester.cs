[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class PerformanceTester
{
    private static readonly string LARGE_IMAGE = "camera.jpg";

    private ImageStats.ILogger logger;
    private string outputFilePath = "testResults.txt";
    private string inputFilePath = "";

    public PerformanceTester(ImageStats.ILogger logger)
    {
        this.logger = logger;
        SetDefaultTestOutputFilePath();
    }

    public void SetOutputFilePath(string outputFilePath)
    {
        this.outputFilePath = outputFilePath;
    }

    public void SetInputFilePath(string inputFilePath)
    {
        this.inputFilePath = inputFilePath;
    }

    public void SetDefaultTestOutputFilePath()
    {
        SetInputFilePath(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../assets/") + LARGE_IMAGE
        );
    }

    public void Test()
    {
        logger.WriteLine("Starting performance tests...");
        logger.WriteLine("Using image: " + inputFilePath);
        TestEdgeDetection();
    }

    public void TestEdgeDetection()
    {
        logger.WriteLine("\nTesting edge detection...");
        logger.WriteLine("Testing with lock");

        var imageReader = new ImageReader(inputFilePath);

        var statsNode = ImageProcessorNS.FilterFactory.Stats(imageReader);
        var outputNode = ImageProcessorNS.FilterFactory.EdgeDetection(statsNode);

        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        outputNode.Read();
        stopwatch.Stop();

        logger.WriteLine("Done! Image processed in " + stopwatch.ElapsedMilliseconds + "ms.");
    }
}