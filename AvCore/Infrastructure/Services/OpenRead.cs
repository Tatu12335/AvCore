using AvCore.Application.Interfaces;

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
