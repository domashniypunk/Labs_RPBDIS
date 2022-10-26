using InspectorLogWebApplication.Models;

namespace InspectorLogWebApplication.Services
{
    public interface ICachedViolations
    {
        public IEnumerable<Violation> GetViolations(int rowNumber);
        public void AddViolations(string cacheKey, int rowNumber);
        public IEnumerable<Violation> GetViolations(string cacheKey, int rowNumber);
    }
}
