using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using carGooBackend.Data;
using carGooBackend.Models;

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObavestenjasController : ControllerBase
    {
        private readonly CarGooDataContext _context;

        public ObavestenjasController(CarGooDataContext context)
        {
            _context = context;
        }

        // GET: api/Obavestenjas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Obavestenja>>> GetObavestenja()
        {
            return await _context.Obavestenja.ToListAsync();
        }

        // GET: api/Obavestenjas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Obavestenja>> GetObavestenja(Guid id)
        {
            var obavestenja = await _context.Obavestenja.FindAsync(id);

            if (obavestenja == null)
            {
                return NotFound();
            }

            return obavestenja;
        }

        // PUT: api/Obavestenjas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObavestenja(Guid id, Obavestenja obavestenja)
        {
            if (id != obavestenja.Id)
            {
                return BadRequest();
            }

            _context.Entry(obavestenja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObavestenjaExists(id))
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

        // POST: api/Obavestenjas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Obavestenja>> PostObavestenja(Obavestenja obavestenja)
        {
            _context.Obavestenja.Add(obavestenja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetObavestenja", new { id = obavestenja.Id }, obavestenja);
        }

        // DELETE: api/Obavestenjas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObavestenja(Guid id)
        {
            var obavestenja = await _context.Obavestenja.FindAsync(id);
            if (obavestenja == null)
            {
                return NotFound();
            }

            _context.Obavestenja.Remove(obavestenja);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        //upload slike
        [HttpPost("upload-notification")]
        public async Task<IActionResult> CreateNotificationWithImage(
        [FromForm] string Naziv,
        [FromForm] string Sadrzaj,
        [FromForm] IFormFile imageFile,
        [FromServices] CloudinaryService cloudinaryService)
        {
            try
            {
                var notification = new Obavestenja
                {
                    Naziv = Naziv,
                    Sadrzaj = Sadrzaj,
                    VremeKreiranja = DateTime.Now
                };

                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageUrl = await cloudinaryService.UploadImageAsync(imageFile, "obavestenja");
                    notification.RepresentImagePath = imageUrl;
                }

                _context.Obavestenja.Add(notification);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Obaveštenje uspešno kreirano",
                    Notification = notification
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Greška pri kreiranju obaveštenja", Error = ex.Message });
            }
        }
        private bool ObavestenjaExists(Guid id)
        {
            return _context.Obavestenja.Any(e => e.Id == id);
        }
    }
}
