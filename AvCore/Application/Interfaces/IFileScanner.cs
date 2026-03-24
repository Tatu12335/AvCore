namespace AvCore.Application.Interfaces
{
    public interface IFileScanner
    {
        public Task<string> ScanFileAsync(string path);
        public Task ProcessZipFileAsync(string file);

    }
}
