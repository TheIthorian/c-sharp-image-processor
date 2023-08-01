using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class InvertFilter : IFilter
{
    public InvertFilter() { }

    public void Process(Bitmap buffer)
    {
        for (int y = 0; y < buffer.Height; y++)
        {
            for (int x = 0; x < buffer.Width; x++)
            {
                var color = buffer.GetPixel(x, y);
                buffer.SetPixel(x, y, Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B));
            }
        }
    }
}