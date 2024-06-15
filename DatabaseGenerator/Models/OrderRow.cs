using System;


namespace DatabaseGenerator.Models
{

    public class OrderRow
    {
        public int RowNumber { get; set; }  // required?
        public int CategoryID { get; internal set; }
        public int SubCategoryID { get; internal set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetPrice { get; set; }
        public decimal UnitCost { get; set; }
    }

}
