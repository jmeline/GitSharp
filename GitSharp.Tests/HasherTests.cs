using Xunit.Abstractions;

namespace GitSharp.Tests;

public class HasherTests(ITestOutputHelper testOutputHelper)
{
    private const string RepoName = "test";
    private const string FileName = "test.txt"; 
    
    [Fact]
    public async Task Serialize()
    {
        var repoPath = Path.Combine(Path.GetTempPath(), RepoName);
        if (Directory.Exists(repoPath))
        {
            Directory.Delete(repoPath, true);
        }
        var initOutput = await GitProcessFunctions.RunGitInitCommand(Path.GetTempPath(), RepoName);
        testOutputHelper.WriteLine(initOutput);

        var testFilePath = Path.Combine(repoPath, FileName);
        if (!File.Exists(testFilePath))
        {
           await File.WriteAllTextAsync(testFilePath, "version 1"); 
        }
        
        var output = await GitProcessFunctions.RunGitHashObjectCommand(repoPath, FileName);
        testOutputHelper.WriteLine(output);

        output = await GitProcessFunctions.RunGitHashObjectInlineCommand(repoPath, "version 1");
        testOutputHelper.WriteLine(output);
        
        // var result = Hasher.HashObject();
    }

    [Fact]
    public async Task Hasher_HashObject_ShouldMatchHashAndGetCorrectPath()
    {
        // Arrange
        const string filename = "hasher_hash_object_test.txt";
        const string data = "Hello, World!";
        await File.WriteAllTextAsync(filename, data);
        
        // Act
        var hash = await Hasher.HashObject(ObjectHeaderTypes.Blob, filename);
        var path = Hasher.GetObjectHashPath(hash);

        // Assert
        testOutputHelper.WriteLine($"Hash: {hash}");
        Assert.Equal("b45ef6fec89518d314f546fd6c3025367b721684", hash);
        Assert.Equal("b4/5ef6fec89518d314f546fd6c3025367b721684", path);
        
        // clean up
        File.Delete(filename);
    }
}