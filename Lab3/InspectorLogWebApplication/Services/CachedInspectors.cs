using InspectorLogWebApplication.Data;
using InspectorLogWebApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InspectorLogWebApplication.Services
{
    public class CachedInspectors : ICachedInspectors
    {
        private readonly InspectorLogContext _context;
        private readonly IMemoryCache _cache;
        public CachedInspectors(InspectorLogContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public void AddInspectors(string cacheKey, int rowNumber)
        {
            IEnumerable<Inspector> inspectors = _context.Inspectors.Take(rowNumber).ToList();
            if (inspectors != null)
            {
                _cache.Set(cacheKey, inspectors, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(272)
                });
            }
        }

        public IEnumerable<Inspector> GetInspectors(int rowNumber)
        {
            return _context.Inspectors.Take(rowNumber).ToList();
        }

        public IEnumerable<Inspector> GetInspectors(string cacheKey, int rowNumber)
        {
            IEnumerable<Inspector> inspectors;
            if (!_cache.TryGetValue(cacheKey, out inspectors))
            {
                inspectors = _context.Inspectors.Take(rowNumber).ToList();
                if (inspectors != null)
                {
                    _cache.Set(cacheKey, inspectors, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(272)));
                }
            }
            return inspectors;
        }
    }
}
