using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InspectorLogWebApplication.Data;
using InspectorLogWebApplication.Models;

namespace InspectorLogWebApplication.Controllers
{
    public class InspectionsController : Controller
    {
        private readonly InspectorLogContext _context;

        public InspectionsController(InspectorLogContext context)
        {
            _context = context;
        }

        // GET: Inspections
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 272)]
        public async Task<IActionResult> Index()
        {
            int rowsNumber = 50;
            var inspectorLogContext = _context.Inspections.Take(rowsNumber).Include(i => i.Enterprise).Include(i => i.Inspector);
            return View(await inspectorLogContext.ToListAsync());
        }

        // GET: Inspections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inspections == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Enterprise)
                .Include(i => i.Inspector)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }

            return View(inspection);
        }
    }
}
