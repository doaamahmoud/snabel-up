﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace snabel_up.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        [MinLength(3, ErrorMessage = "Name must be greater than 3 char")]
        public string Name { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        //[RegularExpression(@"^\w+\.(png|jpg)$")]
        public byte[] Image { get; set; }
        public string Description { get; set; }

        [ForeignKey("SupCategory")]
        public int SupCategory_Id  { get; set; }

        [JsonIgnore]
        public virtual SupCategory SupCategory { get; set; }

       
    }
}
