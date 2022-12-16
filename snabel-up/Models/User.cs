using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace snabel_up.Models
{
    public class User:IdentityUser
    {

        [Required]
        public string UserName { get; set; } = String.Empty;

        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
