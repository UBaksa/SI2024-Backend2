namespace carGooBackend.Models.DTO
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public string UserId { get; set; }
        public Guid CompanyId { get; set; }

        public List<string> Roles { get; set; }

    }
}
