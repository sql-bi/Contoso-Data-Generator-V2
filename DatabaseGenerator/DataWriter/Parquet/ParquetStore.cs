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

    public class ParquetStore
    {

        // ---------------------------------------------------------
        public int StoreKey { get; set; }
        public int StoreCode { get; set; }
        public int GeoAreaKey { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime? OpenDate { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime? CloseDate { get; set; }
        public string Description { get; set; }
        public int? SquareMeters { get; set; }
        public string Status { get; set; }
        // ---------------------------------------------------------

        public static ParquetStore FromStore(Store s)
        {
            return new ParquetStore()
            {
                StoreKey = s.StoreID,
                StoreCode = s.StoreCode,
                GeoAreaKey = s.GeoAreaID,
                CountryCode = s.CountryCode,
                CountryName = s.Country,
                State = s.State,
                OpenDate = s.OpenDate,
                CloseDate = s.CloseDate,
                Description = s.Description,
                SquareMeters = s.SquareMeters,
                Status = s.Status,
            };
        }

        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("StoreKey",      "integer",    false),
                        DeltaField.GetInstance("StoreCode",     "integer",    false),
                        DeltaField.GetInstance("GeoAreaKey",    "integer",    false),
                        DeltaField.GetInstance("CountryCode",   "string",     true),
                        DeltaField.GetInstance("CountryName",   "string",     true),
                        DeltaField.GetInstance("State",         "string",     true),
                        DeltaField.GetInstance("OpenDate",      "timestamp",  true),
                        DeltaField.GetInstance("CloseDate",     "timestamp",  true),
                        DeltaField.GetInstance("Description",   "string",     true),
                        DeltaField.GetInstance("SquareMeters",  "integer",    true),
                        DeltaField.GetInstance("Status",        "string",     true),
                    }
            };
            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }

}
