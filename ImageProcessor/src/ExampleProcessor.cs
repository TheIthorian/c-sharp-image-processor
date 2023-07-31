using System.Drawing;
using System.Drawing.Imaging;

public static class ExampleProcessor
{
    public static void Run()
    {
        // Load an existing image
        Bitmap originalImage = new Bitmap("../image.jpg");

        // Create a new bitmap with the same dimensions
        Bitmap modifiedImage = new Bitmap(originalImage.Width, originalImage.Height);

        // Process each pixel and invert the colors
        ImageProcessor.ProcessPixels(originalImage, modifiedImage);

        // Save the modified image to a new file
        originalImage.Save("../output.jpg", ImageFormat.Jpeg);
    }
}
