using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SobelCalculator : BaseSobelCalculator
{
    private readonly BitmapData bufferData;

    public SobelCalculator(Bitmap buffer) : base(buffer)
    {
        bufferData = buffer.LockBits(
            new Rectangle(0, 0, this.buffer.Width, this.buffer.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb
        );
    }

    public new void Release()
    {
        buffer.UnlockBits(bufferData);
    }

    protected override float CalculateFactor(int x, int y)
    {
        if (bufferData == null)
        {
            throw new InvalidOperationException("BitmapData is null");
        }

        // var color = Color.FromArgb(
        //    Marshal.ReadInt32(bufferData.Scan0, (y * bufferData.Stride) + (x * 4))
        // );

        // Can assume that the image is black and white, so we can read the red chanel only
        var redChanel = Marshal.ReadInt32(bufferData.Scan0, (y * bufferData.Stride) + (x * 4)) >> 16;
        return redChanel & 0xFF; // Mask out alpha channel
    }


}