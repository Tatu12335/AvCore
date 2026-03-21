using System.IO.Compression;

namespace AvCore.Application.Interfaces
{
    public interface IZipArcvhiveService
    {
        public Task<ZipArchive> OpenZipArchive(FileStream fileStream);
        public string HandleTempRoot(string path);

    }
}
