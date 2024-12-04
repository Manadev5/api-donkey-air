using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_donkey_air.Models;

namespace api_donkey_air.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeparturesController : ControllerBase
    {
        private readonly DonkeyAirContext _context;

        public DeparturesController(DonkeyAirContext context)
        {
            _context = context;
        }

        // GET: api/Departures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departure>>> GetDeparture()
        {
            return await _context.Departure.ToListAsync();
        }

        // GET: api/Departures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Departure>> GetDeparture(long id)
        {
            var departure = await _context.Departure.FindAsync(id);

            if (departure == null)
            {
                return NotFound();
            }

            return departure;
        }

        // PUT: api/Departures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeparture(long id, Departure departure)
        {
            if (id != departure.IdDeparture)
            {
                return BadRequest();
            }

            _context.Entry(departure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartureExists(id))
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

        // POST: api/Departures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Departure>> PostDeparture(Departure departure)
        {
            _context.Departure.Add(departure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeparture", new { id = departure.IdDeparture }, departure);
        }

        // DELETE: api/Departures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeparture(long id)
        {
            var departure = await _context.Departure.FindAsync(id);
            if (departure == null)
            {
                return NotFound();
            }

            _context.Departure.Remove(departure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartureExists(long id)
        {
            return _context.Departure.Any(e => e.IdDeparture == id);
        }
    }
}
