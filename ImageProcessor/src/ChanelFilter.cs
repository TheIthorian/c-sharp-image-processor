using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class ChanelFilter : IFilter
{
    private int? red = null;
    private int? green = null;
    private int? blue = null;
    private int? alpha = null;

    public enum Chanel
    {
        Red,
        Green,
        Blue,
        Alpha
    }

    public ChanelFilter() { }

    public ChanelFilter Set(Chanel chanel, int value)
    {
        switch (chanel)
        {
            case Chanel.Red:
                red = value;
                break;
            case Chanel.Green:
                green = value;
                break;
            case Chanel.Blue:
                blue = value;
                break;
            case Chanel.Alpha:
                alpha = value;
                break;
        }
        return this;
    }

    public void Process(Bitmap buffer)
    {
        for (int x = 0; x < buffer.Width; x++)
        {
            for (int y = 0; y < buffer.Height; y++)
            {
                var color = buffer.GetPixel(x, y);

                var newColor = Color.FromArgb(
                    alpha ?? color.A,
                    red ?? color.R,
                    green ?? color.G,
                    blue ?? color.B
                );

                buffer.SetPixel(x, y, newColor);
            }
        }
    }
}