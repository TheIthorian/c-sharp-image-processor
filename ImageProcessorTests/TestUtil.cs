using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
static class TestUtil
{
    public static Bitmap MakeImage(int width, int height)
    {
        return new Bitmap(width, height);
    }

    public static void Fill(Bitmap image, int r = 255, int g = 255, int b = 255, int a = 255)
    {
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                image.SetPixel(x, y, Color.FromArgb(a, r, g, b));
            }
        }
    }

    public static void AssertEqual(Bitmap image1, Bitmap image2)
    {
        Assert.AreEqual(image1.Width, image2.Width);
        Assert.AreEqual(image1.Height, image2.Height);

        for (int x = 0; x < image1.Width; x++)
        {
            for (int y = 0; y < image1.Height; y++)
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

    public static string GetAssetsPath()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../assets");
    }

    public static void SaveImage(Bitmap image, string testName, string ext = "png")
    {
        var path = Path.Combine(GetAssetsPath(), testName + "." + ext);
        image.Save(path, ImageFormat.Png);
    }

    public static Bitmap LoadImage(string testName, string ext = "png")
    {
        var path = Path.Combine(GetAssetsPath(), testName + "." + ext);
        Console.WriteLine(path);
        return (Bitmap)Image.FromFile(path);
    }

    public static Bitmap LoadStandardImage()
    {
        return LoadImage("standardImage");
    }
}