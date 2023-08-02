using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

class TestUtil
{
    public static Bitmap MakeImage(int width, int height)
    {
        return new Bitmap(width, height);
    }

    public static void Fill(Bitmap image, int r = 255, int g = 255, int b = 255, int a = 255)
    {
        for (int x = 1; x < image.Width; x++)
        {
            for (int y = 1; y < image.Height; y++)
            {
                image.SetPixel(x, y, Color.FromArgb(a, r, g, b));
            }
        }
    }

    public static void AssertEqual(Bitmap image1, Bitmap image2)
    {

        Assert.AreEqual(image1.Width, image2.Width);
        Assert.AreEqual(image1.Height, image2.Height);

        for (int x = 1; x < image1.Width; x++)
        {
            for (int y = 1; y < image1.Height; y++)
            {
                var color1 = image1.GetPixel(x, y);
                var color2 = image2.GetPixel(x, y);

                if (color1 != color2)
                {
                    Assert.Fail($"Images differ at pixel ({x}, {y}): {color1} != {color2}");
                }
            }
        }
    }

    public static void SaveImage(Bitmap image, string testName)
    {
        image.Save(
            $"C:/Programming/Misc_Sites/c-sharp-image-processor/ImageProcessorTests/assets/{testName}.png",
            ImageFormat.Png
        );
    }

    public static Bitmap LoadImage(string testName)
    {
        return (Bitmap)Image.FromFile($"C:/Programming/Misc_Sites/c-sharp-image-processor/ImageProcessorTests/assets/{testName}.png");
    }
}