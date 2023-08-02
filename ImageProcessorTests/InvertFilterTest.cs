using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessorNS;

namespace ImageProcessorTests;

[TestClass]
public class InvertFilterTest
{
    [TestMethod]
    public void TestInvertsImageColors()
    {
        var testImage = TestUtil.MakeImage(50, 50);
        TestUtil.Fill(testImage, 0, 100, 200);

        var invertFilter = FilterFactory.From(FilterFactory.Filter.Invert);
        invertFilter.Process(testImage);

        TestUtil.AssertEqual(
            testImage,
            TestUtil.LoadImage("InvertFilterTest.TestInvertsImageColorsResult")
        );

    }
}