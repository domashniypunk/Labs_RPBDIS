using InspectorLogWebApplication.Models;

namespace InspectorLogWebApplication.Services
{
    public interface ICachedInspections
    {
        public IEnumerable<Inspection> GetInspections(int rowNumber);
        public void AddInspections(string cacheKey, int rowNumber);
        public IEnumerable<Inspection> GetInspections(string cacheKey, int rowNumber);
    }
}
