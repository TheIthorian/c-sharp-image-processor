using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SobelCalculator : SobelFilter.ISobelCalculator
{
    public static bool UseBitmapLock = true;

    private static readonly int[] DEFAULT_HORIZONTAL_CONVOLUTION_MATRIX = {
        1, 2, 1,
        0, 0, 0,
        -1, -2, -1
    };

    private static readonly int[] DEFAULT_VERTICAL_CONVOLUTION_MATRIX = {
        1, 0, -1,
        2, 0, -2,
        1, 0, -1
    };

    private static readonly int[,] ADJACENT_DIRECTIONS = {
        { -1, -1 }, { 0, -1 }, { 1, -1 },
        { -1, 0 }, {0 , 0}, { 1, 0 },
        { -1, 1 }, { 0, 1 }, { 1, 1 }
    };

    private int[] horizontalConvolutionMatrix = DEFAULT_HORIZONTAL_CONVOLUTION_MATRIX;
    private int[] verticalConvolutionMatrix = DEFAULT_VERTICAL_CONVOLUTION_MATRIX;

    private readonly Bitmap buffer;
    private readonly BitmapData? bufferData;

    public SobelCalculator(Bitmap buffer)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        this.buffer = new Bitmap(buffer);
        bufferData = UseBitmapLock ? this.buffer.LockBits(
            new Rectangle(0, 0, this.buffer.Width, this.buffer.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb
        ) : null;
    }

    public void Release()
    {
        if (bufferData != null)
        {
            buffer.UnlockBits(bufferData);
        }
    }

    public void SetHorizontalSobelMatrix(int[] horizontalConvolutionMatrix)
    {
        this.horizontalConvolutionMatrix = horizontalConvolutionMatrix;
    }

    public void SetVerticalSobelMatrix(int[] verticalConvolutionMatrix)
    {
        this.verticalConvolutionMatrix = verticalConvolutionMatrix;
    }

    public float CalculateHorizontalFactor(int x, int y)
    {
        return CalculateFactor(x, y, horizontalConvolutionMatrix);
    }

    public float CalculateVerticalFactor(int x, int y)
    {
        return CalculateFactor(x, y, verticalConvolutionMatrix);
    }

    private float CalculateFactor(int x, int y, int[] matrix)
    {
        float factor = 0;

        for (int i = 0; i < ADJACENT_DIRECTIONS.GetLength(0); i++)
        {
            var matrixFactor = matrix[i];
            var adjacentX = ADJACENT_DIRECTIONS[i, 0] + x;
            var adjacentY = ADJACENT_DIRECTIONS[i, 1] + y;

            adjacentX = Math.Max(0, Math.Min(buffer.Width - 1, adjacentX));
            adjacentY = Math.Max(0, Math.Min(buffer.Height - 1, adjacentY));

            // TODO: Find the edge for each color channel
            factor += matrixFactor * (UseBitmapLock
                ? CalculateFactorWithLockBits(adjacentX, adjacentY)
                : CalculateFactorWithoutLockBits(adjacentX, adjacentY));
        }

        return factor;
    }

    private float CalculateFactorWithLockBits(int x, int y)
    {
        if (bufferData == null)
        {
            throw new InvalidOperationException("BitmapData is null");
        }

        // var color = Color.FromArgb(
        //    Marshal.ReadInt32(bufferData.Scan0, (y * bufferData.Stride) + (x * 4))
        // );

        // Can assume that the image is black and white, so we can read the red chanel only
        var redChanel = Marshal.ReadInt32(bufferData.Scan0, (y * bufferData.Stride) + (x * 4)) >> 16;
        return redChanel & 0xFF; // Mask out alpha channel
    }

    private float CalculateFactorWithoutLockBits(int x, int y)
    {
        var adjacentColor = buffer.GetPixel(x, y);

        var factor = (
            adjacentColor.R +
            adjacentColor.G +
            adjacentColor.B
        ) / 3;

        return factor;
    }
}