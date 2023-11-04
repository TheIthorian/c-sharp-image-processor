using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;


[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class ResizeImage
{
    private Image image;

    public ResizeImage(Image image)
    {
        this.image = image;
    }

    public Image Resize(int? width, int? height)
    {
        var intWidth = width ?? 0;
        var intHeight = height ?? 0;

        if (intWidth == 0 && intHeight == 0) return image;
        if (intWidth != 0 && intHeight != 0) return ResizeWithKnownWidthAndHeight(intWidth, intHeight);
        if (intWidth != 0) return Width(intWidth);
        return Height(intHeight);
    }

    public Image Width(int width)
    {
        double resolution = (double)image.Size.Width / (double)image.Size.Height;
        return Resize(width, (int)(width / resolution));
    }

    public Image Height(int height)
    {
        double resolution = (double)image.Size.Width / (double)image.Size.Height;
        return Resize((int)(height * resolution), height);
    }

    private Image ResizeWithKnownWidthAndHeight(int width, int height)
    {
        Console.WriteLine("Resizing image with width=" + width + "; height=" + height);
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }
}