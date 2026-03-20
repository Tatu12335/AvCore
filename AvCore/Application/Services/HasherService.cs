using AvCore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AvCore.Application.Services
{
    public class HasherService : IHasher
    {
        public async Task <string> HashFunc(string filepath,FileInfo fileInfo)
        {
            if (!File.Exists(filepath)) throw new FileNotFoundException();

            try
            {
                using var sha256 = SHA256.Create();

                // Open File for read, allow other processes to read/write while we compute the hash
                using var fS = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Ensure position is at the start
                if (fS.CanSeek) fS.Position = 0;

                // Compute hash async
                byte[] hashValue = await sha256.ComputeHashAsync(fS);

                string hashHex = Convert.ToHexString(hashValue);

                return hashHex;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
