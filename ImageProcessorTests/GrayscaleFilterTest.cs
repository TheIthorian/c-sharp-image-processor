using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessorNS;

namespace ImageProcessorTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class GrayscaleFilterTest
{
    [TestMethod]
    public void TestShouldGrayscaleImage()
    {
        var testImage = TestUtil.LoadStandardImage();

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Gray_Scale);
        invertFilter.Process(testImage);

        TestUtil.AssertEqual(
            testImage,
            TestUtil.LoadImage("GrayscaleFilterTest.TestShouldGrayscaleImage")
        );
    }
}