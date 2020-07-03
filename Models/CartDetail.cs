using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class CartDetail
    {
        [Required]
        [MaxLength(36)]
        public string CartId { get; set; }

        [Required]
        [MaxLength(36)]
        public string ProductId { get; set; }

        [Required]
        public int Qty { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}