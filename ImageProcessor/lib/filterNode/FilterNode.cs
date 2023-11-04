public class FilterNode : FilterNode.IReadable, FilterNode.IReader
{
    public IReadable? input;
    public FilterNode? output;

    public bool isDisabled = false;
    public readonly string name = "FilterNode";

    private IFilter? filter;

    public interface IReadable
    {
        System.Drawing.Bitmap Read();
    }

    public interface IReader
    {
        IReader ConnectInput(IReadable source);
        IReader DisconnectInput();
    }

    public FilterNode(IFilter filter)
    {
        this.filter = filter;
    }

    public IReader ConnectInput(IReadable input)
    {
        this.input = input;
        return this;
    }

    public IReader DisconnectInput()
    {
        input = null;
        return this;
    }

    public FilterNode ConnectOutput(FilterNode output)
    {
        this.output = output;
        this.output.ConnectInput(this);
        return this;
    }

    public FilterNode DisconnectOutput()
    {
        output?.DisconnectInput();
        output = null;
        return this;
    }

    public System.Drawing.Bitmap Read()
    {
        if (input == null)
        {
            throw new AppExceptions.NoInputFileProvided("Unable to read from input: Input not defined");
        }

        var imageData = input.Read();

        if (filter != null && !isDisabled)
        {
            filter.Process(imageData);
        }

        return imageData;
    }

    public void SetFilter(IFilter filter)
    {
        this.filter = filter;
    }

    public List<FilterNode> GetFilterGraph()
    {
        var filterGraph = new List<FilterNode>();

        FilterNode? curr = this;
        while (curr != null)
        {
            filterGraph.Add(curr);
            curr = curr.output;
        }

        return filterGraph;
    }
}