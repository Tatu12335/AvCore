namespace AvCore.Application.Interfaces
{
    public interface IFileScanner
    {
        public Task ScanFileAsync(string path);
        public Task ProcessZipFileAsync(string file);

    }
}
