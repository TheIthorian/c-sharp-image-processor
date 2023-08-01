using System.Drawing.Imaging;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class ImageWriter : FilterNode.IReader
{
    private readonly string filePath;
    private FilterNode.IReadable? input;

    public ImageWriter(string filePath)
    {
        this.filePath = filePath;
    }

    public FilterNode.IReader ConnectInput(FilterNode.IReadable input)
    {
        this.input = input;
        return this;
    }

    public FilterNode.IReader DisconnectInput()
    {
        this.input = null;
        return this;
    }

    public void Write()
    {
        if (input == null)
        {
            throw new AppException("Unable to write to file: No input provided");
        }

        var buffer = input.Read();
        buffer.Save(filePath, ImageFormat.Jpeg);
    }
}