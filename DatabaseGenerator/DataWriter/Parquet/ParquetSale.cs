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
    public class ParquetSale
    {

        // -------------------------------------------------------------------------
        public long OrderKey { get; set; }
        public int LineNumber { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime OrderDate { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime DeliveryDate { get; set; }
        public int CustomerKey { get; set; }
        public int StoreKey { get; set; }
        public int ProductKey { get; set; }
        public int Quantity { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal UnitPrice { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal NetPrice { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal UnitCost { get; set; }
        public string CurrencyCode { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal ExchangeRate { get; set; }
        // -------------------------------------------------------------------------


        public static ParquetSale FromSale(Sale sale)
        {
            return new ParquetSale()
            {
                OrderKey = sale.OrderKey,
                LineNumber = sale.LineNumber,
                OrderDate = sale.OrderDate,
                DeliveryDate = sale.DeliveryDate,
                CustomerKey = sale.CustomerKey,
                StoreKey = sale.StoreKey,
                ProductKey = sale.ProductKey,
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                NetPrice = sale.NetPrice,
                UnitCost = sale.UnitCost,
                CurrencyCode = sale.CurrencyCode,
                ExchangeRate = sale.ExchangeRate
            };
        }


        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("OrderKey",      "long",             false),
                        DeltaField.GetInstance("LineNumber",    "integer",          false),
                        DeltaField.GetInstance("OrderDate",     "timestamp",        false),
                        DeltaField.GetInstance("DeliveryDate",  "timestamp",        false),
                        DeltaField.GetInstance("CustomerKey",   "integer",          false),
                        DeltaField.GetInstance("StoreKey",      "integer",          false),
                        DeltaField.GetInstance("ProductKey",    "integer",          false),
                        DeltaField.GetInstance("Quantity",      "integer",          false),
                        DeltaField.GetInstance("UnitPrice",     "decimal(20,5)",    false),
                        DeltaField.GetInstance("NetPrice",      "decimal(20,5)",    false),
                        DeltaField.GetInstance("UnitCost",      "decimal(20,5)",    false),
                        DeltaField.GetInstance("CurrencyCode",  "string",           true),
                        DeltaField.GetInstance("ExchangeRate",  "decimal(20,5)",    false),
                    }
            };
            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }

}
