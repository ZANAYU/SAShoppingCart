using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(36)]
        public string ProductId { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(140)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Activation> Activations { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<CartDetail> CartDetails { get; set; }

    }
}