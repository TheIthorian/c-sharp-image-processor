using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SobelCalculatorWithoutLock : BaseSobelCalculator
{
    private Bitmap buffer;
    public SobelCalculatorWithoutLock(Bitmap buffer) : base(buffer.Width, buffer.Height)
    {
        this.buffer = buffer;
    }

    protected override float CalculateFactor(int x, int y)
    {
        var adjacentColor = buffer.GetPixel(x, y);

        var factor = (
            adjacentColor.R +
            adjacentColor.G +
            adjacentColor.B
        ) / 3;

        return factor;
    }
}