using Microsoft.EntityFrameworkCore;

namespace api_donkey_air.Models
{
    public class DonkeyAirContext: DbContext
    {
        public DonkeyAirContext(DbContextOptions<DonkeyAirContext> options): base(options)
        {
                
        }


        public DbSet<Departure> Departure { get; set; }
        public DbSet<Destination> Destination { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

    }
}
