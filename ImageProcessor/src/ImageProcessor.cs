using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public static class ImageProcessor
{
    public static void ProcessPixels(Bitmap bitmap)
    {
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                Color pixelColor = bitmap.GetPixel(x, y);

                var newPixelColor = Color.FromArgb(
                    255 - pixelColor.R,
                    255 - pixelColor.G,
                    255 - pixelColor.B
                );

                // Set the pixel in the modified image
                bitmap.SetPixel(x, y, newPixelColor);
            }
        }
    }
}