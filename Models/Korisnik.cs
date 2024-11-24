using Microsoft.AspNetCore.Identity;

namespace carGooBackend.Models
{
    public class Korisnik : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid PreduzeceId { get; set; }
        public virtual Preduzece Preduzece { get; set; }

        
    }
}
