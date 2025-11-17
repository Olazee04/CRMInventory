using CrmInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
            var allMetExpenses = _context.MetExpenses
                .Include(x => x.User) // 👈 This loads the User details
                .ToList();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditExpense(MetExpense model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = new SelectList(_context.Users, "UserId", "Name", model.UserId);
                return View(model);
            }

            if (model.Id == 0)
                _context.MetExpenses.Add(model);
            else
                _context.MetExpenses.Update(model);

            _context.SaveChanges();

            TempData["SuccessMessage"] = "✅ Expense saved successfully!";
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
            // ✅ Total Users
            var totalUsers = _context.Users.Count();
            ViewBag.TotalUsers = totalUsers;

            // ✅ Total Expenses (sum of all)
            var totalExpenses = _context.MetExpenses.Any()
                ? _context.MetExpenses.ToList().Sum(e => e.Value)
                : 0;
            ViewBag.TotalExpenses = totalExpenses;

            
            var highestExpense = _context.MetExpenses.Any()
                ? _context.MetExpenses.ToList().Max(e => e.Value)
                : 0;
            ViewBag.HighestExpense = highestExpense;

            
            var latestExpenses = _context.MetExpenses
                .Include(e => e.User)
                .OrderByDescending(e => e.Id)
                .Take(5)
                .ToList();
            ViewBag.LatestExpenses = latestExpenses;

           
            var userTotals = _context.Users
                .Include(u => u.MetExpenses)
                .Select(u => new
                {
                    UserName = u.Name,
                    Total = u.MetExpenses!.Any()
                        ? u.MetExpenses!.Sum(e => e.Value)
                        : 0
                })
                .ToList();
            ViewBag.UserTotals = userTotals;

            return View();
        }



    }
}
