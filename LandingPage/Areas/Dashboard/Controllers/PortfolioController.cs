using LandingPage.Data;
using LandingPage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandingPage.Areas.Dashboard.Controllers
{
    [Area("dashboard")]
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly AppDbContext _context;

        public PortfolioController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PortfolioController
        public IActionResult Index()
        {
            var portfolioCount = _context.Portfolios.Count();
            ViewBag.portfolioCount = portfolioCount;
            var portfolio = _context.Portfolios.ToList();
            return View(portfolio);
        }

        // GET: PortfolioController/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);

            if (portfolio == null)
                return NotFound();

            return View(portfolio);
        }

       
        [HttpGet]
        public IActionResult Create()
        {
            var portfolioCount = _context.Portfolios.Count();
            if (portfolioCount >= 6)
                return RedirectToAction("Index");

            return View();
        }

     
        [HttpPost]
        public IActionResult Create(Portfolio portfolio, IFormFile NewPhoto)
        {
            if(portfolio.Title != null || portfolio.Subtitle !=null || NewPhoto !=null)
            {
                var fileExtation = Path.GetExtension(NewPhoto.FileName);
                if (fileExtation != ".jpg")
                {
                    ViewBag.PhotoError = "Yalniz jpg formati qebul olunur";
                    return View();
                }
                string myPhoto = Guid.NewGuid().ToString() + Path.GetExtension(NewPhoto.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/Portfolio", myPhoto);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    NewPhoto.CopyTo(stream);
                };
                portfolio.PhotoURL = "admin/Img/Portfolio/" + myPhoto;
                _context.Portfolios.Add(portfolio);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.EmptyError = "Bosluqlari doldurun";
                return View();
            }
            
        }

        // GET: PortfolioController/Edit/5
        public IActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();

            var portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            if (portfolio == null)
                return NotFound();

            return View(portfolio);
        }

        // POST: PortfolioController/Edit/5
        [HttpPost]
        public IActionResult Edit(Portfolio portfolio, IFormFile NewPhoto, string? oldPhoto)
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/admin/Img/Portfolio", myPhoto);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    NewPhoto.CopyTo(stream);
                };
                portfolio.PhotoURL = "admin/Img/Portfolio/" + myPhoto;
            }
            else
            {
                portfolio.PhotoURL = oldPhoto;
            }

            _context.Portfolios.Update(portfolio);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: PortfolioController/Delete/5
        public IActionResult Delete(int id)
        {
            var portfolio = _context.Portfolios.FirstOrDefault(x => x.Id == id);
            if (portfolio == null)
                return RedirectToAction("Index");

            return View();
        }

        // POST: PortfolioController/Delete/5
        [HttpPost]
        public IActionResult Delete(Portfolio portfolio)
        {
            if (portfolio == null)
                return RedirectToAction("Index");

            _context.Portfolios.Remove(portfolio);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
