Console.WriteLine("Hello, GitSharp!");

if (args.Length > 0)
{
    if (args[0].Equals("init", StringComparison.CurrentCultureIgnoreCase))
    {
        var repoName = args[1];
        ArgumentException.ThrowIfNullOrEmpty(repoName);
        
        Directory.CreateDirectory(repoName);
        Console.WriteLine($"Created repository directory: {repoName}");
        
        var repoGitPath = Path.Combine(repoName, ".git");
        Directory.CreateDirectory(repoGitPath);
        Console.WriteLine($"Created git directory in repository: {repoGitPath}");

        var gitFolders = new[]
        {
            "hooks",
            "info",
            "objects",
            "objects/info",
            "objects/pack",
            "refs",
            "refs/heads",
            "refs/tags"
        };

        foreach (var dir in gitFolders)
        {
            var dirPath = Path.Combine(repoGitPath, dir);
            Directory.CreateDirectory(dirPath);
        }

        var headFilePath = Path.Combine(repoGitPath, "HEAD");
        await File.WriteAllBytesAsync(headFilePath, "ref: refs/heads/master\r\n"u8.ToArray());

        Console.WriteLine($"Initialized empty Git repository in {Path.GetFullPath(repoGitPath)}");
    }
} 
