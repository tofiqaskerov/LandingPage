using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize]
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var featureCount = _context.Features.Count();
            ViewBag.featureCount = featureCount;
            var feature = _context.Features.ToList();
            return View(feature);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var feature = _context.Features.FirstOrDefault(x => x.Id == id);

            if (feature == null)
                return NotFound();

            return View(feature);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var featureCount = _context.Features.Count();
            if (featureCount >= 6)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Feature feature)
        {
            if(feature.Title !=null && feature.Subtitle !=null && feature.Icon != null)
            {
                _context.Features.Add(feature);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.EmptyError = "Bosluqlari Doldurun !!";
                return View();
            }
            
        }

        // GET: AboutController/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();

            var feature = _context.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null)
                return NotFound();

            return View(feature);
        }

        // POST: AboutController/Edit/5
        [HttpPost]

        public IActionResult Edit(Feature feature)
        {
            _context.Features.Update(feature);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AboutController/Delete/5
        public IActionResult Delete(int id)
        {
            var feature = _context.Features.FirstOrDefault(x => x.Id == id);
            if (feature == null)
                return RedirectToAction("Index");

            return View();
        }

        // POST: AboutController/Delete/5
        [HttpPost]
        public IActionResult Delete(Feature feature)
        {
            if (feature == null)
                return RedirectToAction("Index");

            _context.Features.Remove(feature);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
