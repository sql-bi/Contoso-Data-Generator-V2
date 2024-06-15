using System;


namespace DatabaseGenerator.Models
{

    public class Product : WeightedItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int SubCategoryID { get; set; }
        public double Price { get; set; }
        public double Cost { get; set; }
        public string ProductCode { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string WeightUnit { get; set; }
        public decimal? Weight { get; set; }

        // --- added at runtime
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public  string SubCategoryName { get; set; }
    }    

}
