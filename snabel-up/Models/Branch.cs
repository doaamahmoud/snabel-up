using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace snabel_up.Models
{
    public class Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3, ErrorMessage = "Name must be greater than 3 char")]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone1 { get; set; }

       
        [DataType(DataType.PhoneNumber)]
        public string Phone2 { get; set; }
    }
}
