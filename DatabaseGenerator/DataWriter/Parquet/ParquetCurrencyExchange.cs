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
    public class ParquetCurrencyExchange
    {

        // ----------------------------------------------------------------
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime Date { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal Exchange { get; set; }
        // ----------------------------------------------------------------


        public static ParquetCurrencyExchange FromCurrencyExchange(CurrencyExchange ce)
        {
            return new ParquetCurrencyExchange()
            {
                Date = ce.Date,
                FromCurrency = ce.FromCurrency,
                ToCurrency = ce.ToCurrency,
                Exchange = (decimal)ce.Exchange,
            };
        }


        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("Date",         "timestamp",     false),
                        DeltaField.GetInstance("FromCurrency", "string",        true),
                        DeltaField.GetInstance("ToCurrency",   "string",        true),
                        DeltaField.GetInstance("Exchange",     "decimal(20,5)", false),
                    }
            };
            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }

}
