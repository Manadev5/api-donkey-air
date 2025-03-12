using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace api_donkey_air.Models
{
    public class User
    {
        [Key]
        public long IdUser { get; set; }
        public string name { get; set; }
        public string password { get; set; }
    }
}
