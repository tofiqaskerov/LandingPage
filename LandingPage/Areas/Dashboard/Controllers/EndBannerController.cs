using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize]
    public class EndBannerController : Controller
    {
        private readonly AppDbContext _context;

        public EndBannerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var endBanner = _context.EndBanners.FirstOrDefault();
            return View(endBanner);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var endBanner = _context.EndBanners.FirstOrDefault(x => x.Id == id);

            if (endBanner == null)
                return NotFound();

            return View(endBanner);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var endBanner = _context.EndBanners.FirstOrDefault();
            if (endBanner != null)
                return RedirectToAction("Index");

            return View();
        }


        [HttpPost]
        public IActionResult Create(EndBanner endBanner, IFormFile NewPhoto)
        {
            if (NewPhoto != null)
            {
                if (endBanner.Title != null)
                {
                    var fileExtation = Path.GetExtension(NewPhoto.FileName);
                    if (fileExtation != ".jpg")
                    {
                        ViewBag.PhotoError = "Yalniz jpg formati qebul olunur";
                        return View();
                    }
                    string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhoto.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/EndBanner", myPhoto);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        NewPhoto.CopyTo(stream);
                    };
                    endBanner.PhotoURL = "admin/Img/EndBanner/" + myPhoto;
                    _context.EndBanners.Add(endBanner);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.EmptyTitle = "Basligi Doldurun";
                    return View();
                }

            }
            else
            {
                ViewBag.EmptyPhoto = "Sekil elave edin";
                return View();
            }

        }

 
        public IActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();

            var endBanner = _context.EndBanners.FirstOrDefault(x => x.Id == id);
            if (endBanner == null)
                return NotFound();

            return View(endBanner);
        }

 
        [HttpPost]
        public IActionResult Edit(EndBanner endBanner, IFormFile NewPhoto, string? oldPhoto)
        {

            if (NewPhoto != null)
            {

                var fileExtation = Path.GetExtension(NewPhoto.FileName);
                if (fileExtation != ".jpg")
                {
                    ViewBag.PhotoError = "Yalniz jpg formati qebul olunur";
                    return View();
                };
                string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhoto.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/EndBanner", myPhoto);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    NewPhoto.CopyTo(stream);
                };
                endBanner.PhotoURL = "admin/Img/EndBanner/" + myPhoto;


            }
            else
                endBanner.PhotoURL = oldPhoto;

            _context.EndBanners.Update(endBanner);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }

        public IActionResult Delete(int id)
        {
            var endBanner = _context.EndBanners.FirstOrDefault(x => x.Id == id);
            if (endBanner == null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public IActionResult Delete(EndBanner endBanner)
        {
            if (endBanner == null)
                return RedirectToAction("Index");

            _context.EndBanners.Remove(endBanner);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
