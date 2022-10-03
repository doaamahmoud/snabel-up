using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace snabel_up.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [MinLength(3, ErrorMessage = "Article Name must be greater than 5 char")]
        public string Name { get; set; }
    
        [Required]
        //[RegularExpression(@"^\w+\.(png|jpg)$")]
        public byte[] Image { get; set; }
        public string Description { get; set; }
     

       
    }
}
