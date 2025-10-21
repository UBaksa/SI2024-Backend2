namespace carGooBackend.Models
{
    public class Obavestenja
    {
        public Guid Id { get; set; }
        public string Naziv { get; set; }
        public string Sadrzaj { get; set; }
        public DateTime VremeKreiranja { get; set; } = DateTime.Now;
        public virtual Korisnik Korisnik { get; set; }
        public string? RepresentImagePath { get; set; }

    }
}
