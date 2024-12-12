using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carGooBackend.Data;
using carGooBackend.Models;
using Microsoft.AspNetCore.Authorization;
using carGooBackend.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreduzecesController : ControllerBase
    {
        private readonly CarGooDataContext _context;
        private readonly UserManager<Korisnik> userManager;

        public PreduzecesController(CarGooDataContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/Preduzeces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PreduzecetToReturnDTO>>> GetPreduzeca()
        {
            var preduzeca = await _context.Preduzeca
                .Include(p => p.Korisnici)
                .Select(p => new PreduzecetToReturnDTO
                {
                    Id = p.Id,
                    CompanyName = p.CompanyName,
                    CompanyState = p.CompanyState,
                    CompanyCity = p.CompanyCity,
                    CompanyMail = p.CompanyMail,
                    CompanyPIB = p.CompanyPIB,
                    CompanyPhone = p.CompanyPhone,
                    Korisnici = p.Korisnici.Select(k => new ReturnKorisnikDTO
                    {
                        FirstName = k.FirstName,
                        LastName = k.LastName,
                        PreduzeceId = k.PreduzeceId
                    }).ToList()
                })
                .ToListAsync();

            return Ok(preduzeca);
        }


        // GET: api/Preduzeces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Preduzece>> GetPreduzece(Guid id)
        {
            var preduzece = await _context.Preduzeca.FindAsync(id);

            if (preduzece == null)
            {
                return NotFound();
            }

            return preduzece;
        }

        // PUT: api/Preduzeces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPreduzece(Guid id, Preduzece preduzece)
        {
            if (id != preduzece.Id)
            {
                return BadRequest();
            }

            _context.Entry(preduzece).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreduzeceExists(id))
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

        // POST: api/Preduzeces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Preduzece>> PostPreduzece([FromBody] CreatePreduzeceDTO createPreduzeceDto)
        {

            try
            {
                // Create a new Preduzece entity
                var preduzece = new Preduzece
                {
                    Id = Guid.NewGuid(),
                    CompanyName = createPreduzeceDto.CompanyName,
                    CompanyState = createPreduzeceDto.CompanyState,
                    CompanyCity = createPreduzeceDto.CompanyCity,
                    CompanyMail = createPreduzeceDto.CompanyMail,
                    CompanyPIB = createPreduzeceDto.CompanyPIB,
                    CompanyPhone = createPreduzeceDto.CompanyPhone
                };

                // Fetch and add Korisnici based on provided IDs
                if (createPreduzeceDto.KorisnikIds != null && createPreduzeceDto.KorisnikIds.Any())
                {
                    foreach (var korisnikId in createPreduzeceDto.KorisnikIds)
                    {
                        // Retrieve the user as a tracked entity from the DbContext
                        var korisnik = await _context.Users.FirstOrDefaultAsync(u => u.Id == korisnikId);
                        if (korisnik == null)
                        {
                            return NotFound(new { Message = $"User with ID {korisnikId} not found." });
                        }

                        // Add the Korisnik to the Preduzece's Korisnici collection
                        preduzece.Korisnici.Add(korisnik);
                    }
                }

                // Add the Preduzece to the context
                _context.Preduzeca.Add(preduzece);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPreduzece", new { id = preduzece.Id }, preduzece);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while creating Preduzece.", Error = ex.Message });
            }
        }

        // DELETE: api/Preduzeces/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Kontroler")]
        public async Task<IActionResult> DeletePreduzece(Guid id)
        {
            var preduzece = await _context.Preduzeca.FindAsync(id);
            if (preduzece == null)
            {
                return NotFound();
            }

            _context.Preduzeca.Remove(preduzece);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PreduzeceExists(Guid id)
        {
            return _context.Preduzeca.Any(e => e.Id == id);
        }
    }
}
