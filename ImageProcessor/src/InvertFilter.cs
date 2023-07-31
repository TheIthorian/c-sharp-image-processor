class InvertFilter : IFilter
{
    public InvertFilter() { }

    public void Process(byte[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (byte)(255 - buffer[i]);
        }
    }
}