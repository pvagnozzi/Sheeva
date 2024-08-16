namespace Sheeva.Core;

using System.Text;

public static class StreamExtensions
{
    public static string AsString(this byte[] data, Encoding? encoding = null) =>
        new((encoding ?? Encoding.Default).GetChars(data));

    public static byte[] AsBytes(this string data, Encoding? encoding = null) =>
        (encoding ?? Encoding.Default).GetBytes(data);

    public static string AsString(this MemoryStream stream, Encoding? encoding = null) =>
        stream.ToArray().AsString(encoding);

}
