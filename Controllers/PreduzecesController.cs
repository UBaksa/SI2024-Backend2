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

namespace carGooBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreduzecesController : ControllerBase
    {
        private readonly CarGooDataContext _context;

        public PreduzecesController(CarGooDataContext context)
        {
            _context = context;
        }

        // GET: api/Preduzeces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Preduzece>>> GetPreduzeca()
        {
            return await _context.Preduzeca.ToListAsync();
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
        public async Task<ActionResult<Preduzece>> PostPreduzece(Preduzece preduzece)
        {
            _context.Preduzeca.Add(preduzece);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPreduzece", new { id = preduzece.Id }, preduzece);
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
