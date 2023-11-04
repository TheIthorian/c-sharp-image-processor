using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class GrayscaleFilter : IFilter
{
    // Grayscale weighting depends on the human eye's sensitivity to different colors.
    private static readonly double R_WEIGHT = 0.3;
    private static readonly double G_WEIGHT = 0.59;
    private static readonly double B_WEIGHT = 0.11;

    public GrayscaleFilter() { }

    public void Process(Bitmap buffer)
    {
        for (int y = 0; y < buffer.Height; y++)
        {
            for (int x = 0; x < buffer.Width; x++)
            {
                var color = buffer.GetPixel(x, y);
                int grayscaleFactor = (int)(R_WEIGHT * color.R + G_WEIGHT * color.G + B_WEIGHT * color.B);
                buffer.SetPixel(
                    x,
                    y,
                    Color.FromArgb(color.A, grayscaleFactor, grayscaleFactor, grayscaleFactor)
                );
            }
        }
    }
}