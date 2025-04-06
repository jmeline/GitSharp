using System.IO.Compression;

namespace GitSharp;

public static class ZLib
{
    public static void CompressFile(string sourceFilePath, string destinationFilePath)
    {
        using var source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read);
        using var destination = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write);
        using var zs = new ZLibStream(destination, CompressionLevel.Optimal, true);

        int bytesRead;
        var buffer = new byte[4096];
        
        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
        {
            zs.Write(buffer, 0, bytesRead);
        }
    }
}