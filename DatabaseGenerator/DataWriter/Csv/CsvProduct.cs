using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter.Csv
{

    internal class CsvProduct
    {

        // ---------------------------------------------------------
        public int ProductKey { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string WeightUnit { get; set; }
        public decimal? Weight { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int CategoryKey { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryKey { get; set; }
        public string SubCategoryName { get; set; }
        // ---------------------------------------------------------


        public static CsvProduct GetFromProduct(Product p)
        {
            return new CsvProduct()
            {
                ProductKey = p.ProductID,
                ProductName = p.ProductName,
                Price = (decimal)p.Price,
                Cost = (decimal)p.Cost,
                ProductCode = p.ProductCode,
                Manufacturer = p.Manufacturer,
                Brand = p.Brand,
                Color = p.Color,
                WeightUnit = p.WeightUnit,
                Weight = p.Weight,
                CategoryKey = p.CategoryID,
                CategoryName = p.CategoryName,
                SubCategoryKey = p.SubCategoryID,
                SubCategoryName = p.SubCategoryName
            };
        }

    }

}
