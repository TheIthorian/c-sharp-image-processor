using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessorNS;

namespace ImageProcessorTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class BackAndWhiteFilterTest
{
    [TestMethod]
    public void TestConvertImageToBlackAndWhite()
    {
        var testImage = TestUtil.LoadStandardImage();

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Black_And_White);
        invertFilter.Process(testImage);

        TestUtil.SaveImage(testImage, "BackAndWhiteFilterTest.TestConvertImageToBlackAndWhite");

        // TestUtil.AssertEqual(
        //     testImage,
        //     TestUtil.LoadImage("BackAndWhiteFilterTest.TestConvertImageToBlackAndWhite")
        // );
    }
}