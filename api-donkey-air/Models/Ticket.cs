using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace api_donkey_air.Models
{
    public class Ticket
    {
        [Key]
        public long IdTicket { get; set; }
        [Required]
        public DateTime departure_date { get; set; }
        [Required]
        public TimeSpan boarding_hour { get; set; }
        [Required]
        public TimeSpan arrival_hour { get; set; }
        [Required]
        public TimeSpan travel_time { get; set; }
        [Required]
        public string travel_number { get; set; }
        [Required]
        public string sit_number { get; set; }
        [Required]
        [ForeignKey("IdDestination")]
        public long IdDestination { get; set; }
        [Required]
        [ForeignKey("IdDeparture")]
        public long IdDeparture { get; set; }
        [Required]
        public decimal price { get; set; }
        [AllowNull]
        public long? user_ticket_id { get; set; }

    }
}
