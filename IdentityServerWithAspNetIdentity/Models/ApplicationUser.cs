using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace IdentityServerWithAspNetIdentity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? TimeOfBirth { get; set; }
        public string BirthCity { get; set; }
        public string BirthCountry { get; set; }
    }
}
