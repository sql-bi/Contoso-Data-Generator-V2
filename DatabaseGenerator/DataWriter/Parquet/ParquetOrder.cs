using DatabaseGenerator.Models;
using Parquet.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;


namespace DatabaseGenerator.DataWriter.Parquet
{

    public class ParquetOrder
    {

        // ---------------------------------------------------------
        public long OrderKey { get; set; }
        public int CustomerKey { get; set; }
        public int StoreKey { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime DT { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime DeliveryDate { get; set; }
        public string CurrencyCode { get; set; }
        // ---------------------------------------------------------


        public static ParquetOrder FromOrder(Order order)
        {
            return new ParquetOrder()
            {
                OrderKey = order.OrderID,
                CustomerKey = order.CustomerID,
                StoreKey = order.StoreID,
                DT = order.DT,
                DeliveryDate = order.DeliveryDate,
                CurrencyCode = order.CurrencyCode
            };
        }


        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("OrderKey",     "long",      false),
                        DeltaField.GetInstance("CustomerKey",  "integer",   false),
                        DeltaField.GetInstance("StoreKey",     "integer",   false),
                        DeltaField.GetInstance("DT",           "timestamp", false),
                        DeltaField.GetInstance("DeliveryDate", "timestamp", false),
                        DeltaField.GetInstance("CurrencyCode", "string",    true),
                    }
            };

            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }

}
