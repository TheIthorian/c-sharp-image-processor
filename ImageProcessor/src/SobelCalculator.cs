using System.Drawing;


[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class SobelCalculator : SobelFilter.ISobelCalculator
{
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

    private readonly Bitmap? buffer;

    public SobelCalculator(Bitmap buffer)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        this.buffer = new Bitmap(buffer);
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

            var adjacentColor = buffer.GetPixel(adjacentX, adjacentY);

            factor += matrixFactor * (
                adjacentColor.R +
                adjacentColor.G +
                adjacentColor.B
            ) / 3;
        }

        return factor;
    }
}