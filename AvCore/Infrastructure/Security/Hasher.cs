using AvCore.Application.Interfaces;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AvCore.Infrastructure.Security
{
    public class Hasher : IHasher
    {
        public async Task<string> HashFunc(FileInfo filepath)
        {
            

            
                
                using var sha256 = SHA256.Create();

                // Open File for read, allow other processes to read/write while we compute the hash
                using var fS = new FileStream(filepath.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Ensure position is at the start
                if (fS.CanSeek) fS.Position = 0;

                // Compute hash async
                byte[] hashValue = await sha256.ComputeHashAsync(fS);

                string hashHex = Convert.ToHexString(hashValue);

                return hashHex;
            
            

        }
    }
}
