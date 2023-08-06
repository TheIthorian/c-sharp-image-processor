class FileLogger : ImageStats.ILogger
{
    private readonly string filename;

    public FileLogger(string filename = "log.txt")
    {
        this.filename = filename;
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
        File.AppendAllText(filename, message + "\n");
    }
}