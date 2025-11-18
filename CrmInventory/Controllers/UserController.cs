using CrmInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrmInventory.Controllers
{
    public class UserController : Controller
    {
        private readonly CrmInventoryDbContext _context;

        public UserController(CrmInventoryDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Display all users
        public IActionResult Index()
        {
            var users = _context.Users
                .Include(u => u.MetExpenses) // include expenses
                .ToList();

            // Add total expense per user using ViewBag or a ViewModel
            ViewBag.UserTotals = users.ToDictionary(
                u => u.UserId,
                u => u.MetExpenses?.Sum(e => e.Value) ?? 0
            );

            return View(users);
        }


        // ✅ GET: Show form to add new user
        public IActionResult Create()
        {
            return View();
        }

        // ✅ POST: Handle form submission (Save new user)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "✅ User added successfully!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["ErrorMessage"] = "❌ An error occurred while saving the user.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "⚠️ Please fill in all required fields correctly.";
            }

            return View(user);
        }

        // ✅ GET: Edit user form
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // ✅ POST: Save edited user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "✅ User updated successfully!";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "⚠️ Please correct the highlighted fields.";
            return View(user);
        }

        // ✅ GET: Confirm delete page
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // ✅ POST: Delete user permanently
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "🗑️ User deleted successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Expenses(int id)
        {
            var user = _context.Users
                .Include(u => u.MetExpenses)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        public IActionResult UserExpenses(int id)
        {
            var user = _context.Users
                .Include(u => u.MetExpenses)
                .FirstOrDefault(u => u.UserId == id);

            if (user == null)
                return NotFound();

            var userExpenses = user.MetExpenses?.ToList() ?? new List<MetExpense>();

            ViewBag.UserName = user.Name;
            ViewBag.TotalExpenses = userExpenses.Sum(e => e.Value);

            return View(userExpenses);
        }

    }
}