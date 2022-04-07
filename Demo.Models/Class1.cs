using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class TokenLoginModel
    {
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
       
    }
 
}
