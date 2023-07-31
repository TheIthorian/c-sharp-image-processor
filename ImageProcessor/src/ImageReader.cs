class ImageReader : FilterNode.IReadable
{
    private readonly string filePath;

    public ImageReader(string filePath)
    {
        this.filePath = filePath;
    }

    public byte[] Read()
    {
        return File.ReadAllBytes(filePath);
    }
}