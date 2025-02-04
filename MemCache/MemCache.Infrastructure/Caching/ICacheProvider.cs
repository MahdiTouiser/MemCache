namespace MemCache.Infrastructure.Caching
{
    public interface ICacheProvider
    {
        Task Clear(string key);
        Task Set(string key, string value, int expireTime);
        Task<string?> Get(string key);
    }

}
