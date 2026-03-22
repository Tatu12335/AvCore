namespace AvCore.Application.Interfaces
{
    public interface IHasher
    {
        public Task<string> HashFunc(string file);
    }
}
