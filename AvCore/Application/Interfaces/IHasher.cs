namespace AvCore.Application.Interfaces
{
    public interface IHasher
    {
        public Task<string> HashFunc(FileInfo filepath);
    }
}
