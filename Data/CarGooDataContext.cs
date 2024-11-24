using carGooBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace carGooBackend.Data
{
    public class CarGooDataContext : IdentityDbContext<Korisnik>
    {
        public CarGooDataContext(DbContextOptions<CarGooDataContext> options ) : base(options)
        {
        }

        public DbSet<Preduzece> Preduzeca { get; set;}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var dispecerRoleId = "8a000e3b-b915-43f1-b90e-b28075ec8cac";
            var prevoznikRoleId = "415a7c65-81dd-4fe3-9c44-9493db860c4b";
            var kontrolerRoleId = "415a7c65-81dd-4fe3-9c44-9493db860c4c";//da promenim sifre!, da stavim hashovane
            var roles = new List<IdentityRole>
    {
		new IdentityRole
        {
            Id = dispecerRoleId,
            ConcurrencyStamp = dispecerRoleId,
            Name="Dispecer",
            NormalizedName="Dispecer".ToUpper()
        },
        new IdentityRole
        {
            Id = prevoznikRoleId,
            ConcurrencyStamp = prevoznikRoleId,
            Name="Prevoznik",
            NormalizedName="Prevoznik".ToUpper()
        },
        new IdentityRole
        {
            Id = kontrolerRoleId,
            ConcurrencyStamp = kontrolerRoleId,
            Name="Kontroler",
            NormalizedName="Kontroler".ToUpper()
        }
    };
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
