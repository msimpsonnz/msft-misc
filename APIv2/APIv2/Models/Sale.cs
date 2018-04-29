using System;

namespace APIv2.Models
{
    public class Sale
    {
        public DateTime SalesDate { get; set; }
        public int LocationId { get; set; }
        public int PosId { get; set; }
        public int EmployeeId { get; set; }
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public double ProductUnits { get; set; }
    }
}
