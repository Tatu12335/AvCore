using AvCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Infrastructure.Services
{
    public class OpenRead : IOpenRead
    {
        public async Task<FileStream> OpenAsync(string filepath)
        {
            // Use Safe FileStream with read sharing
            try
            {
                using var fS = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return fS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
