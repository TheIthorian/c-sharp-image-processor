using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class SobelFilter : IFilter
{
    private ISobelCalculator? sobelCalculator;

    public interface ISobelCalculator
    {
        float CalculateHorizontalFactor(int x, int y);
        float CalculateVerticalFactor(int x, int y);
    }

    public SobelFilter() { }

    public void Process(Bitmap buffer)
    {
        sobelCalculator = new SobelCalculator(buffer);

        for (int x = 0; x < buffer.Width; x++)
        {
            for (int y = 0; y < buffer.Height; y++)
            {
                float verticalFactor = sobelCalculator.CalculateVerticalFactor(x, y) / 255;
                float horizontalFactor = sobelCalculator.CalculateHorizontalFactor(x, y) / 255;

                float sobelFactor = (Math.Abs(verticalFactor) + Math.Abs(horizontalFactor)) / 2;
                // An alternative is to take the geometric mean:
                // float resultingFactor = (float)Math.Sqrt(verticalFactor * verticalFactor + horizontalFactor * horizontalFactor);

                // Clamp max value.
                if (sobelFactor > 1) sobelFactor = 1;

                var color = buffer.GetPixel(x, y);
                var r = (int)(sobelFactor * color.R);
                var g = (int)(sobelFactor * color.G);
                var b = (int)(sobelFactor * color.B);

                var newColor = Color.FromArgb(color.A, r, g, b);

                buffer.SetPixel(x, y, newColor);
            }
        }
    }
}