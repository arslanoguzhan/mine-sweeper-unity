using System.Threading.Tasks;

interface IFileManager
{
    Task<bool> FileExistsAsync(string fileName);

    Task<bool> FileWriteAsync(string fileName, string fileContents);

    Task<string> FileReadAsync(string fileName);
}