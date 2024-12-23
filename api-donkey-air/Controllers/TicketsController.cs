using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_donkey_air.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Mysqlx.Connection;

namespace api_donkey_air.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly DonkeyAirContext _context;

        public TicketsController(DonkeyAirContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicket()
        {
            return await _context.Ticket.ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTickets(long idDeparture, long idDestination)
        {
            var tickets = new List<Ticket>();
            try
            {
                // Connexion à la base de données
                using (MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync();

                    using (MySqlCommand cmd = new MySqlCommand("GetTicketsByDepartureAndDestination", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Ajout des paramètres
                        cmd.Parameters.AddWithValue("@p_IdDeparture", idDeparture);
                        cmd.Parameters.AddWithValue("@p_IdDestination", idDestination);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Créez un objet Ticket basé sur les résultats
                                var ticket = new Ticket
                                {
                                    IdTicket = reader.GetInt64("IdTicket"),
                                    IdDeparture = reader.GetInt64("IdDeparture"),
                                    IdDestination = reader.GetInt64("IdDestination"),

                                    // Ajoutez ici d'autres propriétés si nécessaire
                                };

                                tickets.Add(ticket);
                            }
                        }
                    }
                }

                if (tickets.Count == 0)
                {
                    return NotFound();
                }

                return tickets;
            }
            catch (Exception ex)
            {
                // Gérer les erreurs
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(long id, Ticket ticket)
        {
            if (id != ticket.IdTicket)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // POST: api/Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.IdTicket }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(long id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(long id)
        {
            return _context.Ticket.Any(e => e.IdTicket == id);
        }
    }
}
