using System;
using System.Collections.Generic;
using System.Text;
//using System.ComponentModel.DataAnnotations;

namespace Blazor.Retail.Shared.Models
{
        public class Product
        {
            //[Key]
            public int ProductId { get; set; }
            public int Segment { get; set; }
            public string Category { get; set; }
            public string Description { get; set; }
            public double ProductPrice { get; set; }
        }
}
