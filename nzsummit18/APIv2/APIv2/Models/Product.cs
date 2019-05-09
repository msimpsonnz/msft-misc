using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIv2.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public int Segment { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double ProductPrice { get; set; }

    }
}
