using System.ComponentModel.DataAnnotations;

namespace api_donkey_air.Models
{
    public class Destination
    {
        [Key]
        public long IdDestination { get; set; }
        public string DesCountry { get; set; }
    }
}
