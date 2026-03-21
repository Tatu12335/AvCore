namespace AvCore.Application.Interfaces
{
    public interface IOpenRead
    {
        public Task<FileStream> OpenAsync(string archive);
    }
}
