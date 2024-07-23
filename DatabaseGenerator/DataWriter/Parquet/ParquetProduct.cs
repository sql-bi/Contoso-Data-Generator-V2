using DatabaseGenerator.Models;
using Parquet.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter.Parquet
{

    public class ParquetProduct
    {

        // ---------------------------------------------------------
        public int ProductKey { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string WeightUnit { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal? Weight { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal Cost { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal Price { get; set; }
        public int CategoryKey { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryKey { get; set; }
        public string SubCategoryName { get; set; }

        // ---------------------------------------------------------


        public static ParquetProduct FromProduct(Product p)
        {
            return new ParquetProduct()
            {
                ProductKey = p.ProductID,
                ProductCode = p.ProductCode,
                ProductName = p.ProductName,
                Manufacturer = p.Manufacturer,
                Brand = p.Brand,
                Color = p.Color,
                WeightUnit = p.WeightUnit,
                Weight = p.Weight,
                Cost = (decimal)p.Cost,
                Price = (decimal)p.Price,
                CategoryKey = p.CategoryID,
                CategoryName = p.CategoryName,
                SubCategoryKey = p.SubCategoryID,
                SubCategoryName = p.SubCategoryName,
            };
        }


        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("ProductKey",      "integer",       false),
                        DeltaField.GetInstance("ProductCode",     "string",        true),
                        DeltaField.GetInstance("ProductName",     "string",        true),
                        DeltaField.GetInstance("Manufacturer",    "string",        true),
                        DeltaField.GetInstance("Brand",           "string",        true),
                        DeltaField.GetInstance("Color",           "string",        true),
                        DeltaField.GetInstance("WeightUnit",      "string",        true),
                        DeltaField.GetInstance("Weight",          "decimal(20,5)", true),
                        DeltaField.GetInstance("Cost",            "decimal(20,5)", false),
                        DeltaField.GetInstance("Price",           "decimal(20,5)", false),
                        DeltaField.GetInstance("CategoryKey",     "integer",       false),
                        DeltaField.GetInstance("CategoryName",    "string",        true),
                        DeltaField.GetInstance("SubCategoryKey",  "integer",       false),
                        DeltaField.GetInstance("SubCategoryName", "string",        true),
                    }
            };

            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }
}
