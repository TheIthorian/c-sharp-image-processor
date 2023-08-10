using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessorNS;

namespace ImageProcessorTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class SobelFilterTest
{
    [TestMethod]
    public void TestHighlightsEdgesOfImage()
    {
        var testImage = TestUtil.LoadStandardImage();

        FilterFactory.From(FilterFactory.Filter.Black_And_White).Process(testImage); // Make image black and white first which make sobel filter more visible

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Sobel);
        invertFilter.Process(testImage);

        TestUtil.AssertEqual(
            testImage,
            TestUtil.LoadImage("SobelFilterTest.TestHighlightsEdgesOfImage")
        );
    }
}