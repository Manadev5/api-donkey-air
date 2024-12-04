using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace api_donkey_air.Models
{
    public class User
    {
        [Key]
        public long IdUser { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}
