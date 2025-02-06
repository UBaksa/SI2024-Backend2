using Microsoft.AspNetCore.Identity;

namespace carGooBackend.Models
{
    public class Korisnik : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid PreduzeceId { get; set; }
        public virtual Preduzece Preduzece { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public string? UserPicture { get; set; }
    }

}
