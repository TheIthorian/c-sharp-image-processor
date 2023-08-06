using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace ImageProcessorTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class SobelCalculatorTest
{
    private static readonly int[] IMAGE_PIXELS = {
        10, 20,
        30, 40
    };

    private static Color Pixel(int intensity) => Color.FromArgb(255, intensity, intensity, intensity);

    private static Bitmap CreateBitmap(int width = 2, int height = 2)
    {
        var bitmap = new Bitmap(width, height);

        for (var i = 0; i < IMAGE_PIXELS.Length; i++)
        {
            Console.WriteLine(i % width + ", " + i / height + ": " + IMAGE_PIXELS[i]);
            bitmap.SetPixel(i % width, i / height, Pixel(IMAGE_PIXELS[i]));
        }

        return bitmap;
    }

    [TestMethod]
    public void CalculatesTheCorrectFactorForFirstElement()
    {
        var bitmap = CreateBitmap();
        var sobelCalculator = new SobelCalculator(bitmap);

        var factor = sobelCalculator.CalculateVerticalFactor(0, 0);

        var expectedFactor =
           10 * 1 + 10 * 0 + 20 * -1
         + 10 * 2 + 10 * 0 + 20 * -2
         + 30 * 1 + 30 * 0 + 40 * -1;

        Assert.AreEqual(expectedFactor, factor);
    }

    [TestMethod]
    public void CalculatesTheCorrectFactorForLastElement()
    {
        var bitmap = CreateBitmap();
        var sobelCalculator = new SobelCalculator(bitmap);

        var factor = sobelCalculator.CalculateVerticalFactor(1, 1);

        var expectedFactor =
            10 * 1 + 20 * 0 + 20 * -1
          + 30 * 2 + 40 * 0 + 40 * -2
          + 30 * 1 + 40 * 0 + 40 * -1;

        Assert.AreEqual(expectedFactor, factor);
    }
}