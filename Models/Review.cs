using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Review
    {
        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(36)]
        public string ProductId { get; set; }

        [Required]
        public DateTime UtcDateTime { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        [MaxLength(140)]
        public string Comment { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}