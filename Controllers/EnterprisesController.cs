using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InspectorLogWebApplication.Data;
using InspectorLogWebApplication.Models;
using Microsoft.Data.SqlClient;

namespace InspectorLogWebApplication.Controllers
{
    public class EnterprisesController : Controller
    {
        private readonly InspectorLogContext _context;

        public EnterprisesController(InspectorLogContext context)
        {
            _context = context;
        }

        // GET: Enterprises
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 272)]
        public async Task<IActionResult> Index(SortState sortOrder,
            string searchBossLastName, string searchAdress)
        {
            int rowsNumber = 50;
            var enterprices = Search(_context.Enterprises.Take(rowsNumber).Include(e => e.OwnershipType), sortOrder, 
                searchBossLastName, searchAdress);
            return View(enterprices.ToList());
        }

        // GET: Enterprises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enterprises == null)
            {
                return NotFound();
            }

            var enterprise = await _context.Enterprises
                .Include(e => e.OwnershipType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enterprise == null)
            {
                return NotFound();
            }

            return View(enterprise);
        }
        private IEnumerable<Enterprise> Search(IEnumerable<Enterprise> enterprises, SortState sortOrder, 
            string searchBossLastName, string searchAdress)
        {
            enterprises = enterprises.Where(e => e.BossLastName.Contains(searchBossLastName ?? "")
                & e.Adress.Contains(searchAdress ?? ""));

            ViewData["BossLastName"] = sortOrder == SortState.BossLastNameAsc ? SortState.BossLastNameDesc : SortState.BossLastNameAsc;
            ViewData["BossFirstName"] = sortOrder == SortState.BossFirstNameAsc ? SortState.BossFirstNameDesc : SortState.BossFirstNameAsc;
            ViewData["BossMiddleName"] = sortOrder == SortState.BossMiddleNameAsc ? SortState.BossMiddleNameDesc : SortState.BossMiddleNameAsc;
            ViewData["Name"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["Adress"] = sortOrder == SortState.AdressAsc ? SortState.AdressDesc : SortState.AdressAsc;
            ViewData["OwnershipType"] = sortOrder == SortState.OwnershipTypeAsc ? SortState.OwnershipTypeDesc : SortState.OwnershipTypeAsc;
            ViewData["BossPhoneNumber"] = sortOrder == SortState.BossPhoneNumberAsc ? SortState.BossPhoneNumberDesc : SortState.BossPhoneNumberAsc;

            enterprises = sortOrder switch
            {
                SortState.BossLastNameAsc => enterprises.OrderBy(e => e.BossLastName),
                SortState.BossLastNameDesc => enterprises.OrderByDescending(e => e.BossLastName),
                SortState.BossFirstNameAsc => enterprises.OrderBy(e => e.BossFirstName),
                SortState.BossFirstNameDesc => enterprises.OrderByDescending(e => e.BossFirstName),
                SortState.BossMiddleNameAsc => enterprises.OrderBy(e => e.BossMiddleName),
                SortState.BossMiddleNameDesc => enterprises.OrderByDescending(e => e.BossMiddleName),
                SortState.BossPhoneNumberAsc => enterprises.OrderBy(e => e.BossPhoneNumber),
                SortState.BossPhoneNumberDesc => enterprises.OrderByDescending(e => e.BossPhoneNumber),
                SortState.NameAsc => enterprises.OrderBy(e => e.Name),
                SortState.NameDesc => enterprises.OrderByDescending(e => e.Name),
                SortState.AdressAsc => enterprises.OrderBy(e => e.Adress),
                SortState.AdressDesc => enterprises.OrderByDescending(e => e.Adress),
                SortState.OwnershipTypeAsc => enterprises.OrderBy(e => e.OwnershipType.Name),
                SortState.OwnershipTypeDesc => enterprises.OrderByDescending(e => e.OwnershipType.Name),
                _ => enterprises.OrderBy(e => e.Id)
            };

            return enterprises;
        }
    }
}