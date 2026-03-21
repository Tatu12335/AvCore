using AvCore.Application.DTOs;

namespace AvCore.Application.Interfaces
{
    public interface IAbuseClient
    {
        public Task<Response> GetAbuseChClient(string hashValue);
    }
}
