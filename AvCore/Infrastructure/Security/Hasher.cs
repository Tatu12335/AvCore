using AvCore.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography;

namespace AvCore.Infrastructure.Security
{
    public class Hasher : IHasher
    {
        private readonly ILogger<Hasher> _logger;
        public Hasher(ILogger<Hasher> logger)
        {  _logger = logger; }
        public async Task<string> HashFunc(string filepath)
        {

            try
            {


                using var sha256 = SHA256.Create();

                // Open File for read, allow other processes to read/write while we compute the hash
                using var fS = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Ensure position is at the start
                if (fS.CanSeek) fS.Position = 0;

                // Compute hash async
                byte[] hashValue = await sha256.ComputeHashAsync(fS);

                string hashHex = Convert.ToHexString(hashValue);

                return hashHex;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, " \"Failed to compute SHA256 for file '{FilePath}' (Exists: {Exists})\", filepath?.FullName, filepath?.Exists);\r\n ");               
                throw; // rethrow to preserve original stack trace"
            }
            

        }
    }
}
