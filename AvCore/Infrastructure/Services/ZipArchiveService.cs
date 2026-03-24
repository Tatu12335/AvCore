using AvCore.Application.Interfaces;
using System.IO.Compression;
using System.Net.Sockets;

namespace AvCore.Infrastructure.Services
{
    public class ZipArchiveService : IZipArcvhiveService
    {
        public async Task<ZipArchive> OpenZipArchive(FileStream fileStream)
        {
            using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read, leaveOpen: false);
            return zipArchive;

        }
        public string HandleTempRoot(string path)
        {
            try
            {
                var tempRoot = Path.Combine(Path.GetTempPath(), "Av_core", Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(tempRoot);
                return tempRoot;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }



        }
        public string CreateDestinationPath(string tempRoot,ZipArchiveEntry entry)
        {
            var destinationPath = Path.GetFullPath(Path.Combine(tempRoot, entry.FullName.Replace('/', Path.DirectorySeparatorChar)));
            if(!destinationPath.StartsWith(Path.GetFullPath(tempRoot), StringComparison.OrdinalIgnoreCase))
            {
                return $"Skipping entry : {entry.FullName}, Invalid path";
            }
            var destinationDir = Path.GetDirectoryName(destinationPath);
            if(string.IsNullOrEmpty(destinationDir)) Directory.CreateDirectory(destinationDir);

            return destinationDir;
        }

        public async Task ExtractFileAsync(string destinationPath, ZipArchiveEntry entry)
        {
            try
            {
                using var entryStream = entry.Open();
                using var destStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await destStream.CopyToAsync(entryStream);
       
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
