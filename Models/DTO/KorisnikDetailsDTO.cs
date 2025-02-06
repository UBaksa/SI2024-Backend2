namespace carGooBackend.Models.DTO
{
    public class KorisnikDetailsDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Languages { get; set; } = new List<string>();
        public string UserPicture { get; set; }

    }
}
