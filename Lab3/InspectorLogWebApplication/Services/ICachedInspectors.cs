using InspectorLogWebApplication.Models;

namespace InspectorLogWebApplication.Services
{
    public interface ICachedInspectors
    {
        public IEnumerable<Inspector> GetInspectors(int rowNumber);
        public void AddInspectors(string cacheKey, int rowNumber);
        public IEnumerable<Inspector> GetInspectors(string cacheKey, int rowNumber);
    }
}
