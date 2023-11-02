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
        if (width == null && height == null) return image;
        if (width != null && height != null) return ResizeWithKnownWidthAndHeight(width ?? 0, height ?? 0);
        if (width != null) return Width(width ?? 0);
        return Height(height ?? 0);
    }

    public Image Width(int width)
    {
        var resolution = image.Size.Width / image.Size.Height;
        return Resize(width, width / resolution);
    }

    public Image Height(int height)
    {
        var resolution = image.Size.Width / image.Size.Height;
        return Resize(height * resolution, height);
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