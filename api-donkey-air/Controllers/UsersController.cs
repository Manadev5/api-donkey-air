using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_donkey_air.Models;
using System.Data.Common;
using System.Data;
using MySql.Data.MySqlClient;
using Mysqlx.Connection;

namespace api_donkey_air.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DonkeyAirContext _context;
        private readonly JWTService _jwtservice;

        public UsersController(DonkeyAirContext context, JWTService JWTService)
        {
            _context = context;
            _jwtservice = JWTService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("connection")]
        public async Task<ActionResult<User>> GetUserFromCredentials(string pName, string pPassword)
        {
            User user =  _context.User.FirstOrDefault(u => u.name == pName);
            if (user == null) {
                throw new Exception("L'utilisateur n'existe pas");
            }
            await _context.User.FindAsync(user.IdUser);

             bool ispasswordVerified = AuthService.VerifyPassword(pPassword, user.password);
            if (!ispasswordVerified)
            {
                throw new Exception("Mot de passe de l'utilisateur incorrect");
            }

             string token =  _jwtservice.GenerateToken(user.name);
            return  Ok( new{ message = "Connexion réussie", token});
                
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.IdUser)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async  Task<IActionResult> PostUser([FromBody] User user)
        {

                if (string.IsNullOrWhiteSpace(user.name) || string.IsNullOrWhiteSpace(user.password))
            {
                return BadRequest("Le nom où le mot de passe n\'est pas reisegné dans le bon format");
            }

            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.name == user.name);
            if (existingUser != null)
            {
                return BadRequest("Un utilisateur avec ce nom existe déjà.");
            }

            //hash password pour la base de données
            string hashedPassword = AuthService.HashPassword(user.password);

            try
            {
                // Connexion à la base de données
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "CreateNewUser";
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Ajout des paramètres
                        var pName = cmd.CreateParameter();
                        pName.ParameterName = "@p_name";
                        pName.Value = user.name;
                        cmd.Parameters.Add(pName);

                        var pPassword = cmd.CreateParameter();
                        pPassword.ParameterName = "@p_password";
                        pPassword.Value = hashedPassword;
                        cmd.Parameters.Add(pPassword);

                        using(var reader = await cmd.ExecuteReaderAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                long newUserId = reader.GetInt64(reader.GetOrdinal("NewUserId"));
                            }
                        }

                    }
                }

                string token = _jwtservice.GenerateToken(user.name);

                return Ok(new { message = "Utilisateur créé avec succès !", token });
            }
            catch (Exception ex)
            {
                // Gérer les erreurs
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.User.Any(e => e.IdUser == id);
        }
    }
}
