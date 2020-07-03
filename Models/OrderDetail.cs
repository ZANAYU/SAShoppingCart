using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class OrderDetail
    {
        [Required]
        [MaxLength(36)]
        public string OrderId { get; set; }

        [Required]
        [MaxLength(36)]
        public string ProductId { get; set; }

        [Required]
        public int Qty { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Activation> Activations { get; set; }

    }
}