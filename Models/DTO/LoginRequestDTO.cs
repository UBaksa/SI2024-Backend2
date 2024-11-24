using System.ComponentModel.DataAnnotations;

namespace carGooBackend.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]

        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }
    }
}
