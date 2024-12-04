using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace api_donkey_air.Models
{
    public class Departure
    {
        [Key]
        public long IdDeparture { get; set; }
        public string country { get; set; }
    }
}
