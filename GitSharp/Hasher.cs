using System.Security.Cryptography;
using System.Text;

namespace GitSharp;

public enum ObjectHeaderTypes
{
    Blob,
    Commit,
    Tag,
    Tree
}

public static class Hasher
{
    public static async Task<string> HashObject(ObjectHeaderTypes objectHeader, string filename)
    {
        ArgumentException.ThrowIfNullOrEmpty(filename, nameof(filename));
        const string nullByte = "\0"; // \x00
        var contents = await File.ReadAllTextAsync(filename);
        var contentLength = contents.Length;
        var type = objectHeader.ToString().ToLower();
        
        // blob <length-of-content>\x00 <content>
        var input = $"{type} {contentLength}{nullByte}{contents}";
        var inputBytes = Encoding.UTF8.GetBytes(input);
        // var hashBytes = SHA256.HashData(inputBytes);
        var hashBytes = SHA1.HashData(inputBytes);
        var hash = Convert.ToHexStringLower(hashBytes);
        return hash;
    }

    public static string GetObjectHashPath(string hash)
    {
        ArgumentException.ThrowIfNullOrEmpty(hash, nameof(hash)); 
        return Path.Combine(hash[..2], hash[2..]);
    } 
}