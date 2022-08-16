using LandingPage.Data;
using LandingPage.Models;
using LandingPage.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LandingPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var banners = _context.Banners.FirstOrDefault();
            var features = _context.Features.ToList();
            var portfolios = _context.Portfolios.ToList();
            var endBanner = _context.EndBanners.FirstOrDefault();
            var testimonials = _context.Testimonials.ToList();
            HomeVM vm = new()
            {
                Banner = banners,
                Features = features,
                Portfolios = portfolios,
                endBanner = endBanner,
                Testimonials = testimonials
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmail(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }   
}