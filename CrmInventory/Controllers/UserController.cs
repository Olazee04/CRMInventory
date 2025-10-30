using CrmInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrmInventory.Controllers
{
    public class UserController : Controller
    {
        private readonly CrmInventoryDbContext _context;

        public UserController(CrmInventoryDbContext context)
        {
            _context = context;
        }

        // GET: Display all users
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: Show form to add new user
        public IActionResult Create()
        {
            return View();
        }
        // GET: Edit user form
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Save edited user
        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Confirm delete page
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Delete user permanently
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        // POST: Handle form submission
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}
