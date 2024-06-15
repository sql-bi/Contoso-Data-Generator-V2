using System;


namespace DatabaseGenerator.Models
{
    public class Category : WeightedItem
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public double[] PricePerCent { get; set; }       
    }
}
