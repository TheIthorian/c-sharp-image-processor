using System.Drawing;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class BlankFilter : IFilter
{
    public BlankFilter() { }

    public void Process(Bitmap buffer) { }
}