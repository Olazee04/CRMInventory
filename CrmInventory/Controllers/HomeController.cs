using CrmInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace CrmInventory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CrmInventoryDbContext _context;

        public HomeController(ILogger<HomeController> logger, CrmInventoryDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MetExpenses()
        {
            var allMetExpenses = _context.MetExpenses.ToList();
            var totalMetExpenses = allMetExpenses.Sum(x => x.Value);

            ViewBag.MetExpenses = totalMetExpenses;
            return View(allMetExpenses);
        }

        public IActionResult About()
        {
            return View();
        }

        // ✅ GET: Create or Edit form
        public IActionResult CreateEditExpense(int? id)
        {
            ViewBag.Users = new SelectList(_context.Users, "UserId", "Name");

            if (id == null)
            {
                // New expense
                return View(new MetExpense());
            }

            // Editing existing expense
            var expense = _context.MetExpenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // ✅ POST: Create or Edit form submission
        [HttpPost]
        public IActionResult CreateEditExpense(MetExpense model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_context.Users, "UserId", "Name");
                return View(model);
            }

            if (model.UserId == 0)
            {
                ModelState.AddModelError("UserId", "Please select a user.");
                ViewBag.Users = new SelectList(_context.Users, "UserId", "Name");
                return View(model);
            }

            if (model.Id == 0)
            {
                // Create new expense
                _context.MetExpenses.Add(model);
            }
            else
            {
                // Edit existing expense
                _context.MetExpenses.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("MetExpenses");
        }

        public IActionResult DeleteMetExpense(int id)
        {
            var MetExpenseInDb = _context.MetExpenses.SingleOrDefault(x => x.Id == id);
            if (MetExpenseInDb != null)
            {
                _context.MetExpenses.Remove(MetExpenseInDb);
                _context.SaveChanges();
            }
            return RedirectToAction("MetExpenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            var totalUsers = _context.Users.Count();
            var totalExpenses = _context.MetExpenses.Count();
            var highestExpense = _context.MetExpenses.Any() ? _context.MetExpenses.Max(e => e.Value) : 0;
            var latestExpenses = _context.MetExpenses
                .OrderByDescending(e => e.Id)
                .Take(5)
                .ToList();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalExpenses = totalExpenses;
            ViewBag.HighestExpense = highestExpense;
            ViewBag.LatestExpenses = latestExpenses;

            return View();
        }
    }
}