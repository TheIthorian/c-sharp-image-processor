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

        var chunks = DivideImage(1);
        Console.WriteLine($"Chunks: {chunks.Count}\n");

        // Parallel.ForEach(chunks, ProcessChunk);
        ApplySobelFilter(chunks[0]);
        // ApplySobelFilter(chunks[1]);
        // ApplySobelFilter(chunks[2]);
        // ApplySobelFilter(chunks[3]);

        // foreach (var chunk in chunks) ApplySobelFilter(chunk);
    }

    private class Chunk
    {

        private static int chunkNo = 0;

        public readonly int id;
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public Chunk(int X, int Y, int Width, int Height)
        {
            id = Interlocked.Increment(ref chunkNo);
            Console.WriteLine($"Chunk {id}: X={X}, Y={Y}, Width={Width}, Height={Height}");
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }
    }

    private List<Chunk> DivideImage(int n)
    {
        int chunkWidth = buffer!.Width / n;
        int chunkHeight = buffer.Height / n;
        int remainderWidth = buffer.Width % n;
        int remainderHeight = buffer.Height % n;

        Console.WriteLine($"buffer width={buffer.Width}; chunk width={chunkWidth}");
        Console.WriteLine($"buffer height={buffer.Height}; chunk height={chunkHeight}");

        var chunks = new List<Chunk>();
        for (int x = 0; x < buffer.Width; x += chunkWidth)
        {
            for (int y = 0; y < buffer.Height; y += chunkHeight)
            {
                chunks.Add(new Chunk(x, y, chunkWidth, chunkHeight));
            }
        }

        if (remainderWidth > 0)
        {
            Console.WriteLine("Adding remainder width");
            chunks.Add(new Chunk(buffer.Width - remainderWidth, 0, remainderWidth, buffer.Height)); // Full height remainder
        }

        if (remainderHeight > 0)
        {
            Console.WriteLine("Adding remainder height");
            chunks.Add(new Chunk(0, buffer.Height - remainderHeight, buffer.Width - remainderWidth, remainderHeight));
        }

        return chunks;
    }

    private void ApplySobelFilter(Chunk chunk)
    {
        int width = chunk.Width;
        int height = chunk.Height;

        Console.WriteLine($"Locking chunk {chunk.id} ({chunk.X}:{chunk.X + chunk.Width}, {chunk.Y}:{chunk.Y + chunk.Height})");

        BitmapData inputImageData = buffer!.LockBits(new Rectangle(chunk.X, chunk.Y, chunk.Width, chunk.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

        int bytesPerPixel = 4;
        int stride = buffer.Width * bytesPerPixel;

        byte[] inputBytes = new byte[chunk.Width * chunk.Height * bytesPerPixel]; // inputImageData is only 1 chunk large
        byte[] outputBytes = new byte[chunk.Width * chunk.Height * bytesPerPixel];


        // Copy input image to inputBytes
        Marshal.Copy(inputImageData.Scan0, inputBytes, 0, inputBytes.Length);

        Console.WriteLine($"inputBytes length={inputBytes.Length}; stride={stride}; width={width}; height={height}");

        var sobelCalculator = new SobelCalculator(inputImageData);

        for (int y = 0; y < chunk.Height; y++)
        {
            for (int x = 0; x < chunk.Width; x++)
            {
                int index = (y * stride / 2) + (x * bytesPerPixel);


                // Not working because if mismatched array sizes
                if (y < chunk.Y || y > chunk.Height + chunk.Y || x < chunk.X || x > chunk.X + chunk.Width)
                {
                    continue;
                }

                float verticalFactor = sobelCalculator!.CalculateVerticalFactor(x, y) / 255;
                float horizontalFactor = sobelCalculator.CalculateHorizontalFactor(x, y) / 255;
                float sobelFactor = (Math.Abs(verticalFactor) + Math.Abs(horizontalFactor)) / 2;

                Console.WriteLine($"x={x}; y={y}; index={index}");
                int magnitudeRed = Math.Min((int)(inputBytes[index + 2] * sobelFactor), 255);

                outputBytes[index + 3] = 255;                // Alpha channel
                outputBytes[index + 2] = (byte)magnitudeRed; // Red channel
                outputBytes[index + 1] = (byte)magnitudeRed; // Green channel
                outputBytes[index] = (byte)magnitudeRed;     // Blue channel
            }
        }

        // Copy outputBytes to image
        Marshal.Copy(outputBytes, 0, inputImageData.Scan0, outputBytes.Length);

        Console.WriteLine($"Done writing chunk {chunk.id}\n");

        buffer.UnlockBits(inputImageData);
    }
}