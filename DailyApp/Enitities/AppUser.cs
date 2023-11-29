using Microsoft.AspNetCore.Identity;

namespace DailyApp.Enitities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? DateofBirth { get; set; }
        public ICollection<Daily> Dailies { get; set; }
    }
}
