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

    public static FilterNode EdgeDetection(FilterNode.IReadable reader)
    {
        var blackAndWhiteFilterNode = FilterNodeFrom(Filter.Black_And_White);
        blackAndWhiteFilterNode.ConnectInput(reader);

        var sobelFilterNode = FilterNodeFrom(Filter.Sobel);
        sobelFilterNode.ConnectInput(blackAndWhiteFilterNode);

        return sobelFilterNode;
    }

    public static FilterNode InvertMirror(FilterNode.IReadable reader)
    {
        var invertFilterNode = FilterNodeFrom(Filter.Invert);
        invertFilterNode.ConnectInput(reader);

        var mirrorFilterNode = FilterNodeFrom(Filter.Mirror);
        mirrorFilterNode.ConnectInput(invertFilterNode);

        return mirrorFilterNode;
    }

    public static FilterNode Stats(FilterNode.IReadable reader)
    {
        var stats = new ImageStats(new FileLogger());
        var filterNode = new FilterNode(stats);
        filterNode.ConnectInput(reader);

        return filterNode;
    }
}