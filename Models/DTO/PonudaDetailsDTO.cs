namespace carGooBackend.Models.DTO
{
    public class PonudaDetailsDTO : PonudeDTO
    {
        public KorisnikDetailsDTO Korisnik { get; set; }
        public PreduzeceDetailsDTO Preduzece { get; set; }
        public Guid IdPreduzeca { get; set; }
        public string IdKorisnika { get; set; }
    }
}
