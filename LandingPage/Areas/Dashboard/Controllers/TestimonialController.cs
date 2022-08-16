using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize]
    public class TestimonialController : Controller
    {
        private readonly AppDbContext _context;

        public TestimonialController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var testimonialCount = _context.Testimonials.Count();
            ViewBag.testimonialCount = testimonialCount;
            var testimonial = _context.Testimonials.ToList();
            return View(testimonial);
        }

       
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var testimonial = _context.Testimonials.FirstOrDefault(x => x.Id == id);

            if (testimonial == null)
                return NotFound();

            return View(testimonial);
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            var testimonialCount = _context.Testimonials.Count();
            if (testimonialCount >= 6)
                return RedirectToAction("Index");

            return View();
        }

      
        [HttpPost]
        public IActionResult Create(Testimonial testimonial, IFormFile NewPhoto)
        {
            if (testimonial.Name != null || testimonial.Description != null || NewPhoto != null)
            {
                var fileExtation = Path.GetExtension(NewPhoto.FileName);
                if (fileExtation != ".jpg")
                {
                    ViewBag.PhotoError = "Yalniz jpg formati qebul olunur";
                    return View();
                }
                string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhoto.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/Testimonial", myPhoto);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    NewPhoto.CopyTo(stream);
                };
                testimonial.PhotoURL = "admin/Img/Testimonial/" + myPhoto;
                _context.Testimonials.Add(testimonial);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.EmptyError = "Bosluqlari doldurun";
                return View();
            }

        }

      
        public IActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();

            var testimonial = _context.Testimonials.FirstOrDefault(x => x.Id == id);
            if (testimonial == null)
                return NotFound();

            return View(testimonial);
        }

    
        [HttpPost]
        public IActionResult Edit(Testimonial testimonial, IFormFile NewPhoto, string? oldPhoto)
        {
            if(testimonial.Name != null || testimonial.Description != null)
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
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/testimonial", myPhoto);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        NewPhoto.CopyTo(stream);
                    };
                    testimonial.PhotoURL = "admin/Img/testimonial/" + myPhoto;
                }
                else
                {
                    testimonial.PhotoURL = oldPhoto;
                }
                _context.Testimonials.Update(testimonial);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.EmptyError = "Bosluqlari doldurun";
                return View();
            }
           

          
        }

        public IActionResult Delete(int id)
        {
            var testimonial = _context.Testimonials.FirstOrDefault(x => x.Id == id);
            if (testimonial == null)
                return RedirectToAction("Index");

            return View();
        }


        [HttpPost]
        public IActionResult Delete(Testimonial testimonial)
        {
            if(testimonial == null)
                return RedirectToAction("Index");

            _context.Testimonials.Remove(testimonial);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
