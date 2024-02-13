using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

class FileManager : IFileManager
{
    public async Task<bool> FileExistsAsync(string fileName)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName);
        var exists = await Task.Run(() => File.Exists(filePath));
        
        return exists;
    }

    public async Task<bool> FileWriteAsync(string fileName, string fileContents)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName);

        try
        {
            await File.WriteAllTextAsync(filePath, fileContents);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {filePath} with exception {e}");
            return false;
        }
    }

    public async Task<string> FileReadAsync(string fileName)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName);
        var contents = "";

        try
        {
            contents = await File.ReadAllTextAsync(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {filePath} with exception {e}");
        }

        return contents;
    }
}