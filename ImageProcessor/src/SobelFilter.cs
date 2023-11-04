using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class SobelFilter : IFilter
{
    private Bitmap? buffer;

    public interface ISobelCalculator
    {
        float CalculateHorizontalFactor(int x, int y);
        float CalculateVerticalFactor(int x, int y);
        void Release();
    }

    public SobelFilter() { }

    public void Process(Bitmap buffer)
    {
        this.buffer = buffer;
        int numberOfChunks = 32;

        Stopwatch watch = new Stopwatch();
        watch.Start();

        var chunks = DivideImage(numberOfChunks);

        var rect = new Rectangle(0, 0, buffer.Width, buffer.Height);
        BitmapData inputImageData = buffer.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

        Parallel.ForEach(chunks, (chunk) => ApplySobelFilter(chunk, inputImageData));

        watch.Stop();
        Console.WriteLine($"Completed in {watch.Elapsed.TotalMilliseconds} using {numberOfChunks} chunks\n");
        Console.WriteLine($"{numberOfChunks}\t{watch.Elapsed.TotalMilliseconds}");

        buffer.UnlockBits(inputImageData);
    }

    private class Chunk
    {

        private static int chunkNo = 0;
        public readonly int Id;
        public int Width;
        public int Height;
        public int Y;
        public int Start { get => Y * Width; }
        public int Length { get => Width * Height; }

        public Chunk(int y, int width, int height)
        {
            Id = Interlocked.Increment(ref chunkNo);
            Y = y;
            Width = width;
            Height = height;
            Console.WriteLine($"Chunk {Id}: y={Y}; width={Width}; height={Height}; start={Start}; length={Length}");
        }
    }

    private List<Chunk> DivideImage(int n)
    {
        int bufferWidth = buffer!.Width;
        int chunkHeight = buffer.Height / n;
        int remainderHeight = buffer.Height % n;

        Console.WriteLine($"buffer height={buffer.Height}; chunk height={chunkHeight}");

        var chunks = new List<Chunk>();
        for (int y = 0; y + chunkHeight <= buffer.Height; y += chunkHeight)
        {
            chunks.Add(new Chunk(y, bufferWidth, chunkHeight));
        }

        if (remainderHeight > 0)
        {
            Console.WriteLine("Adding remainder height");
            chunks.Add(new Chunk(buffer.Height - remainderHeight, bufferWidth, remainderHeight));
        }

        return chunks;
    }

    private void ApplySobelFilter(Chunk chunk, BitmapData inputImageData)
    {
        int bytesPerPixel = 4;
        int stride = buffer!.Width * bytesPerPixel;

        byte[] inputBytes = new byte[chunk.Length * bytesPerPixel]; // inputImageData is only 1 chunk large
        byte[] outputBytes = new byte[chunk.Length * bytesPerPixel];

        // Copy input image to inputBytes
        Marshal.Copy(inputImageData.Scan0 + (chunk.Start * bytesPerPixel), inputBytes, 0, chunk.Length * bytesPerPixel);

        var sobelCalculator = new SobelCalculator(inputImageData);

        for (int y = 0; y < chunk.Height; y++)
        {
            for (int x = 0; x < chunk.Width; x++)
            {
                int index = (y * stride) + (x * bytesPerPixel);

                float verticalFactor = sobelCalculator!.CalculateVerticalFactor(x, y + chunk.Y) / 255;
                float horizontalFactor = sobelCalculator.CalculateHorizontalFactor(x, y + chunk.Y) / 255;
                float sobelFactor = (Math.Abs(verticalFactor) + Math.Abs(horizontalFactor)) / 2;

                int magnitudeRed = Math.Min((int)(inputBytes[index + 2] * sobelFactor), 255);

                outputBytes[index + 3] = 255;                // Alpha channel
                outputBytes[index + 2] = (byte)magnitudeRed; // Red channel
                outputBytes[index + 1] = (byte)magnitudeRed; // Green channel
                outputBytes[index] = (byte)magnitudeRed;     // Blue channel
            }
        }

        // Copy outputBytes to image
        Marshal.Copy(outputBytes, 0, inputImageData.Scan0 + (chunk.Start * bytesPerPixel), chunk.Length * bytesPerPixel);
    }
}
