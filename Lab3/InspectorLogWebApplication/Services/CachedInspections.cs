using InspectorLogWebApplication.Data;
using InspectorLogWebApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace InspectorLogWebApplication.Services
{
    public class CachedInspections : ICachedInspections
    {
        private readonly InspectorLogContext _context;
        private readonly IMemoryCache _cache;
        public CachedInspections(InspectorLogContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddInspections(string cacheKey, int rowNumber)
        {
            IEnumerable<Inspection> storages = _context.Inspections.Take(rowNumber).ToList();
            if (storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(272)
                });
            }
        }

        public IEnumerable<Inspection> GetInspections(int rowNumber)
        {
            return _context.Inspections.Take(rowNumber).ToList();
        }

        public IEnumerable<Inspection> GetInspections(string cacheKey, int rowNumber)
        {
            IEnumerable<Inspection> storages;
            if (!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = _context.Inspections.Take(rowNumber).ToList();
                if (storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(272)));
                }
            }
            return storages;
        }
    }
}
