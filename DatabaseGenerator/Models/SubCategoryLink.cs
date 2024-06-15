using System;


namespace DatabaseGenerator.Models
{
    public class SubCategoryLink
    {
        public int SubCategoryID { get; set; }
        public int LinkedSubCategoryID { get; set; }
        public double PerCent { get; set; }
    }
}
