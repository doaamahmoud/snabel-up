using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace snabel_up.Models
{
    public class Login:IdentityUser
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
