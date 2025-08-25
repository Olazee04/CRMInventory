using CrmInventory.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult CreateEditExpense(int? id)
        {
            if(id != null)
            {

                //editing -> load an expenses by id
                var MetExpenseInDb = _context.MetExpenses.SingleOrDefault(MetExpense => MetExpense.Id == id);
                return View(MetExpenseInDb);

            }
            return View();
        }

        public IActionResult DeleteMetExpense(int id)
        {
            var MetExpenseInDb = _context.MetExpenses.SingleOrDefault(MetExpense => MetExpense.Id == id);
            _context.MetExpenses.Remove(MetExpenseInDb);
            _context.SaveChanges();
            return View();
        }



        public IActionResult CreateEditExpenseForm(MetExpense model)
        {
            if(model.Id == 0)
            {

                //Create
                _context.MetExpenses.Add(model);
            } else
            {
                //Editing
                _context.MetExpenses.Update(model);

            }

            _context.SaveChanges();

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
    }
}
