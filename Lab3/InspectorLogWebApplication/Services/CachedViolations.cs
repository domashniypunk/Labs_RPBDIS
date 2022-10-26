using InspectorLogWebApplication.Data;
using InspectorLogWebApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InspectorLogWebApplication.Services
{
    public class CachedViolations : ICachedViolations
    {
        private readonly InspectorLogContext _context;
        private readonly IMemoryCache _cache;
        public CachedViolations(InspectorLogContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public void AddViolations(string cacheKey, int rowNumber)
        {
            IEnumerable<Violation> storages = _context.Violations.Take(rowNumber).ToList();
            if (storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(272)
                });
            }
        }

        public IEnumerable<Violation> GetViolations(int rowNumber)
        {
            return _context.Violations.Take(rowNumber).ToList();
        }

        public IEnumerable<Violation> GetViolations(string cacheKey, int rowNumber)
        {
            IEnumerable<Violation> storages;
            if (!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = _context.Violations.Take(rowNumber).ToList();
                if (storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(272)));
                }
            }
            return storages;
        }
    }
}
