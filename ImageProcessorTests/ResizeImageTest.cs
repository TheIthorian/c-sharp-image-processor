using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace ImageProcessorTests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class ResizeImageTest
{
    [TestMethod]
    public void TestResizeImage()
    {
        var testImage = TestUtil.LoadStandardImage();

        var resizeImage = new ResizeImage(testImage);
        var newImageBitmap = (Bitmap)resizeImage.Resize(100, 50);

        TestUtil.AssertEqual(
            newImageBitmap,
            TestUtil.LoadImage("ResizeImageTest.TestResizeImage")
        );
    }
}