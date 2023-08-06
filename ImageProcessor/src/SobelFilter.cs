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
                // An alternative is to take the geometric mean, which increases the contract in edges:
                // float sobelFactor = (float)Math.Sqrt(verticalFactor * verticalFactor + horizontalFactor * horizontalFactor);

                var color = buffer.GetPixel(x, y);
                var newColor = Color.FromArgb(
                    color.A,
                    Math.Min((int)(sobelFactor * color.R), 255),
                    Math.Min((int)(sobelFactor * color.G), 255),
                    Math.Min((int)(sobelFactor * color.B), 255)
                );

                buffer.SetPixel(x, y, newColor);
            }
        }
    }
}