using AvCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Infrastructure.Services
{
    public class ZipArchiveService : IZipArcvhiveService
    {
        public async Task <ZipArchive>OpenZipArchive(FileStream fileStream)
        {
            using var zipArchive = new ZipArchive(fileStream,ZipArchiveMode.Read,leaveOpen:false);
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
    }
}
