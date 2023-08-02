using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessorNS;

namespace ImageProcessorTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class InvertFilterTest
{
    [TestMethod]
    public void TestInvertsImageColors()
    {
        var testImage = TestUtil.LoadStandardImage();

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Invert);
        invertFilter.Process(testImage);

        TestUtil.AssertEqual(
            testImage,
            TestUtil.LoadImage("InvertFilterTest.TestInvertsImageColors")
        );
    }
}