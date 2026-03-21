using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Application.Interfaces
{
    public interface IZipArcvhiveService
    {
        public Task <ZipArchive> OpenZipArchive(FileStream fileStream);
        public string HandleTempRoot(string path);

    }
}
