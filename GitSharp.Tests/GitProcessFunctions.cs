using System.Diagnostics;

namespace GitSharp.Tests;

public static class GitProcessFunctions
{
    public static async Task<string> RunGitInitCommand(string path, string repo)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "git",
            Arguments = $"init {repo}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = path
        };

        var result = Process.Start(startInfo);
        ArgumentNullException.ThrowIfNull(result);
        var output = await result.StandardOutput.ReadToEndAsync();
        return output;
    }
    
    public static async Task<string> RunGitHashObjectCommand(string filePath, string filename)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "git",
            Arguments = $"hash-object -w {filename}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = filePath
        };

        var result = Process.Start(startInfo);
        ArgumentNullException.ThrowIfNull(result);
        var output = await result.StandardOutput.ReadToEndAsync();
        return output;
    }
    
    public static async Task<string> RunGitHashObjectInlineCommand(string filePath, string content)
    {
        var echo = new Process
        {
            StartInfo = new()
            {
                FileName = "echo",
                Arguments = "version 1",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = filePath
            }
        };
        var gitHashObject = new Process
        {
            StartInfo = new()
            {
                FileName = "git",
                Arguments = "hash-object -w --stdin",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = filePath
            }
        };

        echo.Start();
        gitHashObject.Start();
        
        
        // pipe echo -> git
        await echo.StandardOutput.BaseStream.CopyToAsync(gitHashObject.StandardInput.BaseStream);
        gitHashObject.StandardInput.Close();

        var output = await gitHashObject.StandardOutput.ReadToEndAsync();
        
        // using var process = Process.Start(startInfo);
        // ArgumentNullException.ThrowIfNull(process);
        //
        // await process.StandardInput.WriteAsync("version 1\n");
        // process.StandardInput.Close();
        //
        // var output = await process.StandardOutput.ReadToEndAsync();
        
        return output.Trim();
    }
}