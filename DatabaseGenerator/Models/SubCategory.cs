using System;


namespace DatabaseGenerator.Models
{
    public class SubCategory : WeightedItem
    {
        public int SubCategoryID { get; set; }
        public int CategoryID { get; set; }
        public string SubCategoryName { get; set; }
    }
}
