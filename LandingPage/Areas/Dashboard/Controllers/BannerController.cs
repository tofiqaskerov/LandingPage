using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;

        public BannerController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var banner = _context.Banners.FirstOrDefault();
            return View(banner);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var banner = _context.Banners.FirstOrDefault(x => x.Id == id);

            if (banner == null)
                return NotFound();

            return View(banner);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var banner = _context.Banners.FirstOrDefault();
            if (banner != null)
                return RedirectToAction("Index");

            return View();
        }


        [HttpPost]
        public IActionResult Create(Banner banner, IFormFile NewPhoto)
        {
            if (NewPhoto != null)
            {
                if (banner.Title != null)
                {
                    var fileExtation = Path.GetExtension(NewPhoto.FileName);
                    if (fileExtation != ".jpg")
                    {
                        ViewBag.PhotoError = "Yalniz jpg formati qebul olunur";
                        return View();
                    }
                    string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhoto.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/Banner", myPhoto);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        NewPhoto.CopyTo(stream);
                    };
                    banner.PhotoURL = "admin/Img/Banner/" + myPhoto;
                    _context.Banners.Add(banner);
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

            var banner = _context.Banners.FirstOrDefault(x => x.Id == id);
            if (banner == null)
                return NotFound();

            return View(banner);
        }

        // POST: BannerController/Edit/5
        [HttpPost]
        public IActionResult Edit(Banner banner, IFormFile NewPhoto, string? oldPhoto)
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/Banner", myPhoto);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    NewPhoto.CopyTo(stream);
                };
                banner.PhotoURL = "admin/Img/Banner/" + myPhoto;


            }
            else
                banner.PhotoURL = oldPhoto;

            _context.Banners.Update(banner);
            _context.SaveChanges();
            return RedirectToAction("Index");


        }


        public IActionResult Delete(int id)
        {
            var banner = _context.Banners.FirstOrDefault(x => x.Id == id);
            if (banner == null)
                return RedirectToAction("Index");

            return View();
        }


        [HttpPost]
        public IActionResult Delete(Banner banner)
        {
            if (banner == null)
                return RedirectToAction("Index");

            _context.Banners.Remove(banner);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
