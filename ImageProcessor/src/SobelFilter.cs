using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
class SobelFilter : IFilter
{
    private ISobelCalculator? sobelCalculator;
    private Bitmap? buffer;
    private BitmapData? bufferData;

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
        sobelCalculator = new SobelCalculator((Bitmap)buffer.Clone());

        var chunks = DivideImage(1);
        Console.WriteLine($"Chunks: {chunks.Count}\n");

        // Parallel.ForEach(chunks, ProcessChunk);

        foreach (var chunk in chunks)
        {
            ApplySobelFilter(chunk);
        }


        sobelCalculator.Release();
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

        if (remainderWidth > 0 && remainderHeight > 0)
        {
            Console.WriteLine("Adding remainder");
            chunks.Add(new Chunk(buffer.Width - remainderWidth, buffer.Height - remainderHeight, remainderWidth, remainderHeight));
        }

        return chunks;
    }

    private void ProcessChunk(Chunk chunk)
    {
        int currentChunkNo = 1;
        Console.WriteLine($"\n{currentChunkNo} Chunk: X={chunk.X}, Y={chunk.Y}, Width={chunk.Width}, Height={chunk.Height}");

        for (int x = 0; x < chunk.Width; x++)
        {
            for (int y = 0; y < chunk.Height; y++)
            {
                var pixelX = x + chunk.X;
                var pixelY = y + chunk.Y;

                float verticalFactor = sobelCalculator!.CalculateVerticalFactor(pixelX, pixelY) / 255;
                float horizontalFactor = sobelCalculator.CalculateHorizontalFactor(pixelX, pixelY) / 255;

                float sobelFactor = (Math.Abs(verticalFactor) + Math.Abs(horizontalFactor)) / 2;
                // An alternative is to take the geometric mean, which increases the contract in edges:
                // float sobelFactor = (float)Math.Sqrt(verticalFactor * verticalFactor + horizontalFactor * horizontalFactor);

                // Get the address of the first line.
                IntPtr ptr = bufferData.Scan0 + (pixelY * bufferData.Stride) + (pixelX * 4);

                // Declare an array to hold the bytes of the bitmap.
                int bytes = Math.Abs(bufferData.Stride) * chunk.Height;
                byte[] rgbValues = new byte[bytes];

                // Copy the RGB values into the array.
                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                // Set every third value to 255. A 24bpp bitmap will look red.  
                // rgbValues[1] = (byte)Math.Min(sobelFactor * rgbValues[1], 255);
                // rgbValues[2] = (byte)Math.Min(sobelFactor * rgbValues[2], 255);
                // rgbValues[3] = (byte)Math.Min(sobelFactor * rgbValues[3], 255);

                // Get the pixel index in the array, for the position x,y
                var pixelIndex = (bufferData.Stride) + (pixelX * 4);
                Console.WriteLine($"Pixel: {pixelX}, {pixelY}, Index: {pixelIndex}");
                rgbValues[0 + pixelIndex] = (byte)Math.Min(sobelFactor * rgbValues[0 + pixelIndex], 255);
                rgbValues[1 + pixelIndex] = (byte)Math.Min(sobelFactor * rgbValues[1 + pixelIndex], 255);
                rgbValues[2 + pixelIndex] = (byte)Math.Min(sobelFactor * rgbValues[2 + pixelIndex], 255);

                // Copy the RGB values back to the bitmap
                System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

                // var color = buffer.GetPixel(pixelX, pixelY);
                // var newColor = Color.FromArgb(
                //     color.A,
                //     Math.Min((int)(sobelFactor * color.R), 255),
                //     Math.Min((int)(sobelFactor * color.G), 255),
                //     Math.Min((int)(sobelFactor * color.B), 255)
                // );
            }
        }

    }

    private void ApplySobelFilter(Chunk chunk)
    {
        int width = chunk.Width;
        int height = chunk.Height;

        Console.WriteLine($"Locking chunk {chunk.id} ({chunk.X}-{chunk.X + chunk.Width}, {chunk.Y}-{chunk.Y + chunk.Height})");

        BitmapData inputImageData = buffer!.LockBits(new Rectangle(chunk.X, chunk.Y, chunk.Width, chunk.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

        int bytesPerPixel = 4;
        int stride = chunk.Width * bytesPerPixel;

        byte[] inputBytes = new byte[width * height * bytesPerPixel];
        byte[] outputBytes = new byte[width * height * bytesPerPixel];

        Marshal.Copy(inputImageData.Scan0, inputBytes, 0, inputBytes.Length);

        Console.WriteLine($"inputBytes length={inputBytes.Length}; stride={stride}");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float verticalFactor = sobelCalculator!.CalculateVerticalFactor(x, y) / 255;
                float horizontalFactor = sobelCalculator.CalculateHorizontalFactor(x, y) / 255;
                float sobelFactor = (Math.Abs(verticalFactor) + Math.Abs(horizontalFactor)) / 2;

                int index = (y * stride) + (x * bytesPerPixel);
                // Console.WriteLine($"x={x}; y={y}; index={index}");
                int magnitudeRed = Math.Min((int)(inputBytes[index + 2] * sobelFactor), 255);

                outputBytes[index + 3] = 255;                // Alpha channel
                outputBytes[index + 2] = (byte)magnitudeRed; // Red channel
                outputBytes[index + 1] = (byte)magnitudeRed; // Green channel
                outputBytes[index] = (byte)magnitudeRed;     // Blue channel
            }
        }

        Marshal.Copy(outputBytes, 0, inputImageData.Scan0, outputBytes.Length);

        Console.WriteLine($"Done writing chunk {chunk.id}\n");

        buffer.UnlockBits(inputImageData);
    }
}