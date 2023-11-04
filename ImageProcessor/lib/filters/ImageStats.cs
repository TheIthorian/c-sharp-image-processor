using System.Drawing;
using System.Drawing.Imaging;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class ImageStats : IFilter
{
    public bool LogToFile { get; set; } = false;
    private readonly ILogger logger;

    public interface ILogger
    {
        void WriteLine(string message);
    }

    public ImageStats(ILogger logger, bool LogToFile = false)
    {
        this.LogToFile = LogToFile;
        this.logger = logger;
    }

    public void Process(Bitmap buffer)
    {
        var width = buffer.Width;
        var height = buffer.Height;
        var pixelCount = width * height;

        var pixelData = buffer.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);

        var pixelBytes = new byte[pixelCount * 4];
        System.Runtime.InteropServices.Marshal.Copy(pixelData.Scan0, pixelBytes, 0, pixelBytes.Length);
        buffer.UnlockBits(pixelData);

        var red = 0;
        var green = 0;
        var blue = 0;
        var alpha = 0;

        for (var i = 0; i < pixelBytes.Length; i += 4)
        {
            red += pixelBytes[i];
            green += pixelBytes[i + 1];
            blue += pixelBytes[i + 2];
            alpha += pixelBytes[i + 3];
        }

        var averageRed = red / pixelCount;
        var averageGreen = green / pixelCount;
        var averageBlue = blue / pixelCount;
        var averageAlpha = alpha / pixelCount;

        var message = "Image stats...\n" +
            $"Image size: {width} x {height}. Total pixel count: " + pixelCount + "\n" +
            $"Average color: R:{averageRed} G:{averageGreen} B:{averageBlue} A:{averageAlpha}\n";

        logger.WriteLine(message);
    }
}