using DatabaseGenerator.Models;
using Parquet.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json;


namespace DatabaseGenerator.DataWriter.Parquet
{

    public class ParquetOrderRow
    {

        // ---------------------------------------------------------
        public long OrderKey { get; set; }
        public int RowNumber { get; set; }
        public int ProductKey { get; set; }
        public int Quantity { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal UnitPrice { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal NetPrice { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal UnitCost { get; set; }
        // ---------------------------------------------------------


        public static ParquetOrderRow FromOrderRow(Order order, OrderRow orderRow)
        {
            return new ParquetOrderRow()
            {
                OrderKey = order.OrderID,
                RowNumber = orderRow.RowNumber,
                ProductKey = orderRow.ProductID,
                Quantity = orderRow.Quantity,
                UnitPrice = orderRow.UnitPrice,
                NetPrice = orderRow.NetPrice,
                UnitCost = orderRow.UnitCost,
            };
        }


        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                {
                     DeltaField.GetInstance("OrderKey",   "long",          false),
                     DeltaField.GetInstance("RowNumber",  "integer",       false),
                     DeltaField.GetInstance("ProductKey", "integer",       false),
                     DeltaField.GetInstance("Quantity",   "integer",       false),
                     DeltaField.GetInstance("UnitPrice",  "decimal(20,5)", false),
                     DeltaField.GetInstance("NetPrice",   "decimal(20,5)", true),
                     DeltaField.GetInstance("UnitCost",   "decimal(20,5)", true),
                }
            };

            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }

}
