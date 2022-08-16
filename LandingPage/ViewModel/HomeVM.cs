using LandingPage.Models;

namespace LandingPage.ViewModel
{
    public class HomeVM
    {
        public Banner Banner { get; set; }
        public EndBanner endBanner { get; set; }
        public List<Feature> Features { get; set; }
        public List<Portfolio> Portfolios { get; set; }
        public List<Testimonial> Testimonials { get; set; } 

    }
}       
