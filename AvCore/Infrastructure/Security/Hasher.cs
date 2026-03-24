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
        { _logger = logger; }
        public async Task<string> HashFunc(string filepath)
        {

            try
            {        

                var sha256 = SHA256.Create();

                // Open File for read, allow other processes to read/write while we compute the hash
                using var fS = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                if (fS.CanSeek)
                {                   
                    fS.Position = 0;                                

                    var hashTask = sha256.ComputeHashAsync(fS); 
                    if(await Task.WhenAny(hashTask, Task.Delay(TimeSpan.FromSeconds(30))) == hashTask)
                    {
                        byte[] hashValue = await hashTask;
                        
                        string hashHex = Convert.ToHexString(hashValue);
         
                        return hashHex;
                    }
                    else
                    {
                        Debug.WriteLine("Hashing lasted for over 30 seconds");
                    }

                    
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error failed to hash {FilePath} ", filepath);
                throw; // rethrow to preserve original stack trace"
            }


        }
    }
}
