using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SobelCalculatorWithoutLock : BaseSobelCalculator
{
    public SobelCalculatorWithoutLock(Bitmap buffer) : base(buffer) { }

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