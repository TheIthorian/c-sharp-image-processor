namespace ImageProcessorNS;


[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class FilterFactory
{
    public enum Filter
    {
        Invert,
        Mirror,
        Sobel,
        Gray_Scale,
        Black_And_White,
        Fourier
    }

    public static IFilter FromArray(Array filterArray)
    {
        Console.WriteLine(filterArray);
        return new InvertFilter();
    }

    public static IFilter From(Filter filter)
    {
        return filter switch
        {
            Filter.Invert => new InvertFilter(),
            Filter.Mirror => new MirrorFilter(),
            Filter.Gray_Scale => new GrayscaleFilter(),
            Filter.Black_And_White => new BlackAndWhiteFilter(),
            Filter.Sobel => new SobelFilter(),
            _ => throw new Exception("Unsupported filter: " + filter.ToString()),
        };
    }

    public static IFilter FromString(string filterName)
    {
        Filter filter = (Filter)Enum.Parse(typeof(Filter), filterName);
        return From(filter);
    }

    public static FilterNode FilterNodeFrom(Filter filter)
    {
        return new FilterNode(From(filter));
    }
}