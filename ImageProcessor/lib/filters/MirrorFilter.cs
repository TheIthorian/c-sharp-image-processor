using System.Drawing;


[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class MirrorFilter : IFilter
{
    private bool _vertical = false;
    private Bitmap? _buffer;

    public MirrorFilter() { }

    public MirrorFilter Vertical()
    {
        _vertical = true;
        return this;
    }

    public MirrorFilter Horizontal()
    {
        _vertical = false;
        return this;
    }


    public void Process(Bitmap buffer)
    {
        // copy image to new buffer
        _buffer = buffer.Clone() as Bitmap;

        if (_buffer == null) throw new Exception("Failed to clone bitmap.");

        for (int y = 1; y < _buffer.Height; y++)
        {
            for (int x = 1; x < _buffer.Width; x++)
            {
                if (_vertical) buffer.SetPixel(x, _buffer.Height - y, _buffer.GetPixel(x, y));
                else buffer.SetPixel(_buffer.Width - x, y, _buffer.GetPixel(x, y));
            }
        }
    }
}