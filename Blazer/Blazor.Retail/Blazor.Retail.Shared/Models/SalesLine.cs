using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Retail.Shared.Models
{
    public class SalesLine
    {
        public SalesLine(DateTime salesDate, int locationId, int posId, int employeeId, int productId, double productPrice, double productUnits)
        {
            SalesDate = salesDate;
            LocationId = locationId;
            PosId = posId;
            EmployeeId = employeeId;
            BasketId = long.Parse((salesDate.ToString("yyyyMMddHHmm") + LocationId + PosId));
            ProductId = productId;
            ProductPrice = productPrice;
            ProductUnits = productUnits;
        }

        public DateTime SalesDate { get; set; }
        public int LocationId { get; set; }
        public int PosId { get; set; }
        public int EmployeeId { get; set; }
        public long BasketId { get; set; }
        public int ProductId { get; set; }
        public double ProductPrice { get; set; }
        public double ProductUnits { get; set; }
    }

}
