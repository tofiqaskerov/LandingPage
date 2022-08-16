using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var contacts = _context.Contacts.ToList();
            return View(contacts);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Id == Id);    
            if(contact == null)
                return RedirectToAction("Index");

            return View();
        }
        [HttpPost]
        public IActionResult Delete(Contact contact)
        {
            if (contact == null)
                return RedirectToAction("Index");

            _context.Contacts.Remove(contact);
            _context.SaveChanges(); 

            return RedirectToAction("Index");   
        }
    }
}
