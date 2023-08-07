[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class PerformanceTester
{
    private static readonly string LARGE_IMAGE = "mountain.jpg";

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

        logger.WriteLine("\nTesting with lock");
        SobelCalculator.UseBitmapLock = true;
        TestEdgeDetection();

        logger.WriteLine("\nTesting without lock");
        SobelCalculator.UseBitmapLock = false;
        TestEdgeDetection();
    }

    public void TestEdgeDetection()
    {
        logger.WriteLine("Testing edge detection...");
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