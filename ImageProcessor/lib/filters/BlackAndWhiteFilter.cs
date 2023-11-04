using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class BlackAndWhiteFilter : IFilter
{
    private static readonly double R_WEIGHT = 0.3;
    private static readonly double G_WEIGHT = 0.59;
    private static readonly double B_WEIGHT = 0.11;

    private static readonly int DEFAULT_THRESHOLD = 110;

    public readonly int threshold = DEFAULT_THRESHOLD;

    public BlackAndWhiteFilter() { }

    public void Process(Bitmap buffer)
    {
        for (int x = 0; x < buffer.Width; x++)
        {
            for (int y = 0; y < buffer.Height; y++)
            {
                Color color = buffer.GetPixel(x, y);
                int grayscaleFactor = (int)(R_WEIGHT * color.R + G_WEIGHT * color.G + B_WEIGHT * color.B);
                int intensity = grayscaleFactor > threshold ? 255 : 0;

                buffer.SetPixel(x, y, Color.FromArgb(color.A, intensity, intensity, intensity));
            }
        }
    }
}