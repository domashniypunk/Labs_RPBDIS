using InspectorLogWebApplication.Data;
using Microsoft.AspNetCore.Mvc;

namespace InspectorLogWebApplication.Controllers
{
    public class InspectorsController : Controller
    {
        public readonly InspectorLogContext _context;
        public InspectorsController(InspectorLogContext context)
        {
            _context = context;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 272)]
        public IActionResult Index()
        {
            int rowsNumber = 50;
            return View(_context.Inspectors.Take(rowsNumber).ToList());
        }
    }
}
