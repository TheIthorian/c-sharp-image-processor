using System.Drawing.Imaging;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class ImageWriter : FilterNode.IReader
{
    private readonly string filePath;
    private ImageFormat outputFormat = ImageFormat.Jpeg;
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
        input = null;
        return this;
    }

    public ImageWriter OutputFormat(string outputFormat)
    {
        this.outputFormat = GetImageFormat(outputFormat);
        return this;
    }

    public void Write(bool deduceOutputFormat = false)
    {
        if (input == null) throw new AppExceptions.NoInputConnected("Unable to write image to file: No input connected.");

        if (deduceOutputFormat) outputFormat = GetImageFormat(filePath.Split('.').Last());

        var buffer = input.Read();
        buffer.Save(filePath, outputFormat);
    }

    private static ImageFormat GetImageFormat(string outputFormat)
    {
        return outputFormat switch
        {
            "jpg" => ImageFormat.Jpeg,
            "png" => ImageFormat.Png,
            "bmp" => ImageFormat.Bmp,
            _ => ImageFormat.Jpeg,
        };
    }
}