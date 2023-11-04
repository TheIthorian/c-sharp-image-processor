using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SobelCalculator : BaseSobelCalculator
{
    private readonly BitmapData bufferData;

    public SobelCalculator(BitmapData bufferData) : base(bufferData.Width, bufferData.Height)
    {
        this.bufferData = bufferData;
    }

    protected override float CalculateFactor(int x, int y)
    {
        if (bufferData == null)
        {
            throw new InvalidOperationException("bufferData is null");
        }

        // var color = Color.FromArgb(
        //    Marshal.ReadInt32(bufferData.Scan0, (y * bufferData.Stride) + (x * 4))
        // );

        // Can assume that the image is black and white, so we can read the red chanel only
        var redChanel = Marshal.ReadInt32(bufferData.Scan0, (y * bufferData.Stride) + (x * 4)) >> 16;
        // var redChanel = imageBytes[(y * stride) + (x * 4)] >> 16;
        return redChanel & 0xFF; // Mask out alpha channel
    }


}