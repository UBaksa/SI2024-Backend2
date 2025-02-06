using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carGooBackend.Data;
using carGooBackend.DTOs;
using carGooBackend.Models;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObavestenjesController : ControllerBase
    {
        private readonly CarGooDataContext _context;

        public ObavestenjesController(CarGooDataContext context)
        {
            _context = context;
        }

        // GET: api/Obavestenjes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObavestenjeDTO>>> GetObavestenje()
        {
            return await _context.Obavestenje
                .Include(o => o.Autor) // Uključuje podatke o autoru
                .Select(o => new ObavestenjeDTO
                {
                    Id = o.Id,
                    Naslov = o.Naslov,
                    DatumObjavljivanja = o.DatumObjavljivanja,
                    Sadrzaj = o.Sadrzaj,
                    ProfilePicture = o.ProfilePicture,
                    AutorFirstName = o.Autor.FirstName,
                    AutorLastName = o.Autor.LastName
                })
                .ToListAsync();
        }

        // GET: api/Obavestenjes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ObavestenjeDTO>> GetObavestenje(Guid id)
        {
            var obavestenje = await _context.Obavestenje
                .Include(o => o.Autor)
                .Select(o => new ObavestenjeDTO
                {
                    Id = o.Id,
                    Naslov = o.Naslov,
                    DatumObjavljivanja = o.DatumObjavljivanja,
                    Sadrzaj = o.Sadrzaj,
                    ProfilePicture = o.ProfilePicture,
                    AutorFirstName = o.Autor.FirstName,
                    AutorLastName = o.Autor.LastName
                })
                .FirstOrDefaultAsync(o => o.Id == id);

            if (obavestenje == null)
            {
                return NotFound();
            }

            return obavestenje;
        }

        // PUT: api/Obavestenjes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObavestenje(Guid id, ObavestenjeDTO obavestenjeDto)
        {
            if (id != obavestenjeDto.Id)
            {
                return BadRequest();
            }

            var obavestenje = await _context.Obavestenje.Include(o => o.Autor).FirstOrDefaultAsync(o => o.Id == id);
            if (obavestenje == null)
            {
                return NotFound();
            }

            // Ažuriranje entiteta
            obavestenje.Naslov = obavestenjeDto.Naslov;
            obavestenje.DatumObjavljivanja = obavestenjeDto.DatumObjavljivanja;
            obavestenje.Sadrzaj = obavestenjeDto.Sadrzaj;
            obavestenje.ProfilePicture = obavestenjeDto.ProfilePicture;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObavestenjeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Obavestenjes
        [HttpPost]
        public async Task<ActionResult<ObavestenjeDTO>> PostObavestenje(ObavestenjeDTO obavestenjeDto)
        {
            // Proveravamo da li korisnik postoji u bazi
            var autor = await _context.Users.FirstOrDefaultAsync(u => u.Id == obavestenjeDto.AutorId);
            if (autor == null)
            {
                return NotFound("Autor sa zadatim ID-jem ne postoji.");
            }

            var obavestenje = new Obavestenje
            {
                Id = Guid.NewGuid(),
                Naslov = obavestenjeDto.Naslov,
                DatumObjavljivanja = DateTime.UtcNow,
                Sadrzaj = obavestenjeDto.Sadrzaj,
                ProfilePicture = obavestenjeDto.ProfilePicture,
                AutorId = obavestenjeDto.AutorId
            };

            _context.Obavestenje.Add(obavestenje);
            await _context.SaveChangesAsync();

            // Mapiranje na DTO za povratak
            var responseDto = new ObavestenjeDTO
            {
                Id = obavestenje.Id,
                Naslov = obavestenje.Naslov,
                DatumObjavljivanja = obavestenje.DatumObjavljivanja,
                Sadrzaj = obavestenje.Sadrzaj,
                ProfilePicture = obavestenje.ProfilePicture,
                AutorFirstName = autor.FirstName,
                AutorLastName = autor.LastName
            };

            return CreatedAtAction("GetObavestenje", new { id = obavestenje.Id }, responseDto);
        }

        // DELETE: api/Obavestenjes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObavestenje(Guid id)
        {
            var obavestenje = await _context.Obavestenje.FindAsync(id);
            if (obavestenje == null)
            {
                return NotFound();
            }

            _context.Obavestenje.Remove(obavestenje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ObavestenjeExists(Guid id)
        {
            return _context.Obavestenje.Any(e => e.Id == id);
        }
    }
}
