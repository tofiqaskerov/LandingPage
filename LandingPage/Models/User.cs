using Microsoft.AspNetCore.Identity;

namespace LandingPage.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
