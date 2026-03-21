using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Application.Interfaces
{
    public interface IFileScanner
    {
        public Task ScanFileAsync(string path);
        public Task ProcessZipFileAsync(string file);

    }
}
