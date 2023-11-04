class VectorTransformation
{
    readonly int Width;
    readonly int Height;

    public class Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public VectorTransformation(int Width, int Height)
    {
        this.Width = Width;
        this.Height = Height;
    }

    public Position IndexToPosition(int index)
    {
        return new Position(index % Width, index / Height);
    }


    public int PositionToIndex(Position position)
    {
        return Height * position.y + position.x;
    }
}