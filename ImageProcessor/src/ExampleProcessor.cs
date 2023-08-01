using System.Drawing;
using System.Drawing.Imaging;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public static class ExampleProcessor
{
    public static void Run()
    {
        // Load an existing image
        var originalImage = new Bitmap("../image.jpg");

        // Process each pixel and invert the colors
        ImageProcessor.ProcessPixels(originalImage);

        // Save the modified image to a new file
        originalImage.Save("../output.jpg", ImageFormat.Jpeg);
    }
}
