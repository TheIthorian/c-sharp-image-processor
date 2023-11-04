[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public abstract class BaseSobelCalculator : SobelFilter.ISobelCalculator
{
    protected static readonly int[] DEFAULT_HORIZONTAL_CONVOLUTION_MATRIX = {
        1, 2, 1,
        0, 0, 0,
        -1, -2, -1
    };

    protected static readonly int[] DEFAULT_VERTICAL_CONVOLUTION_MATRIX = {
        1, 0, -1,
        2, 0, -2,
        1, 0, -1
    };

    protected static readonly int[,] ADJACENT_DIRECTIONS = {
        { -1, -1 }, { 0, -1 }, { 1, -1 },
        { -1, 0 }, {0 , 0}, { 1, 0 },
        { -1, 1 }, { 0, 1 }, { 1, 1 }
    };

    protected int[] horizontalConvolutionMatrix = DEFAULT_HORIZONTAL_CONVOLUTION_MATRIX;
    protected int[] verticalConvolutionMatrix = DEFAULT_VERTICAL_CONVOLUTION_MATRIX;

    protected readonly int width;
    protected readonly int height;

    protected BaseSobelCalculator(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Release()
    {
        return;
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

            adjacentX = Math.Max(0, Math.Min(width - 1, adjacentX));
            adjacentY = Math.Max(0, Math.Min(height - 1, adjacentY));

            // TODO: Find the edge for each color channel
            factor += matrixFactor * CalculateFactor(adjacentX, adjacentY);
        }

        return factor;
    }

    protected abstract float CalculateFactor(int x, int y);
}