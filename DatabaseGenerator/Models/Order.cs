using System;
using System.Collections.Generic;


namespace DatabaseGenerator.Models
{

    public class Order
    {
        public long OrderID { get; set; }
        public int CustomerID { get; set; }
        public int StoreID { get; set; }
        public DateTime DT { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CurrencyCode { get; set; }
        public List<OrderRow> Rows { get; set; }
    }

}
