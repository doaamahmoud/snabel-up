using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace snabel_up.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3, ErrorMessage = "Name must be greater than 3 char")]
        public string Name { get; set; }

    }
}
