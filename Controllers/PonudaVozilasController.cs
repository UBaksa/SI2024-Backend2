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
    [Authorize(Roles = "Prevoznik")]
    [Authorize(Roles = "Kontroler")]
    public class PonudaVozilasController : ControllerBase
    {
        private readonly CarGooDataContext _context;

        public PonudaVozilasController(CarGooDataContext context)
        {
            _context = context;
        }

        // GET: api/PonudaVozilas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PonudaVozila>>> GetPonudaVozila()
        {
            return await _context.PonudaVozila.ToListAsync();
        }

        // GET: api/PonudaVozilas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PonudaVozila>> GetPonudaVozila(Guid id)
        {
            var ponudaVozila = await _context.PonudaVozila.FindAsync(id);

            if (ponudaVozila == null)
            {
                return NotFound();
            }

            return ponudaVozila;
        }

        // PUT: api/PonudaVozilas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPonudaVozila(Guid id, PonudaVozila ponudaVozila)
        {
            if (id != ponudaVozila.Id)
            {
                return BadRequest();
            }

            _context.Entry(ponudaVozila).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PonudaVozilaExists(id))
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

        // POST: api/PonudaVozilas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PonudaVozila>> PostPonudaVozila(PonudaVozila ponudaVozila)
        {
            _context.PonudaVozila.Add(ponudaVozila);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPonudaVozila", new { id = ponudaVozila.Id }, ponudaVozila);
        }

        // DELETE: api/PonudaVozilas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePonudaVozila(Guid id)
        {
            var ponudaVozila = await _context.PonudaVozila.FindAsync(id);
            if (ponudaVozila == null)
            {
                return NotFound();
            }

            _context.PonudaVozila.Remove(ponudaVozila);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PonudaVozilaExists(Guid id)
        {
            return _context.PonudaVozila.Any(e => e.Id == id);
        }
    }
}
