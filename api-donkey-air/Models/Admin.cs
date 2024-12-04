using System.ComponentModel.DataAnnotations;


namespace api_donkey_air.Models
{
    public class Admin
    {
        [Key]
        public long IdAdmin { get; set; }
        public string name { get; set; }
        public string password { get; set; }

    }
}
