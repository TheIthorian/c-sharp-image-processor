using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class ImageReader : FilterNode.IReadable
{
    private readonly string filePath;

    public ImageReader(string filePath)
    {
        this.filePath = filePath;
    }

    public Bitmap Read()
    {
        return (Bitmap)Bitmap.FromFile(filePath);
    }
}