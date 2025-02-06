namespace carGooBackend.Models.DTO
{
    public class ReturnKorisnikDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid PreduzeceId { get; set; }

        public string PhoneNumber { get;set; }
        public string Mail { get; set; }
    }
}
