using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace carGooBackend.Models
{
    public class Obavestenje
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Naslov { get; set; }

        [Required]
        public DateTime DatumObjavljivanja { get; set; }

        [Required]
        public string Sadrzaj { get; set; }

        [Required]
        [ForeignKey(nameof(Autor))] 
        public string AutorId { get; set; }

        public virtual Korisnik Autor { get; set; } 
        public string? ProfilePicture { get; set; }
    }
}
