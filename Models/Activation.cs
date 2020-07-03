using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Activation
    {
        [Required]
        [MaxLength(36)]
        public string ProductId { get; set; }

        [Required]
        public string Code { get; set; }

        [MaxLength(36)]
        public string OrderId { get; set; }
    }
}