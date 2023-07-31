class FilterFactory
{
    public enum FILTER
    {
        INVERT,
        MIRROR,
        SOBEL,
        GRAY_SCALE,
        BLACK_AND_WHITE,
        FOURIER
    }

    public static IFilter FromArray(Array filterArray)
    {
        Console.WriteLine(filterArray);
        return new InvertFilter();
    }

    public static IFilter FromString(string filterName)
    {
        return new InvertFilter();
    }
}