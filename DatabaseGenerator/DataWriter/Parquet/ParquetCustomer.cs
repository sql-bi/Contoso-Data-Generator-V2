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

    public class ParquetCustomer
    {

        // ----------------------------------------------------------------
        public int CustomerKey { get; set; }
        public int GeoAreaKey { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime? StartDT { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime? EndDT { get; set; }
        public string Continent { get; set; }
        public string Gender { get; set; }
        public string Title { get; set; }
        public string GivenName { get; set; }
        public string MiddleInitial { get; set; }
        public string Surname { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateFull { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CountryFull { get; set; }
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public string Company { get; set; }
        public string Vehicle { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal Latitude { get; set; }
        [ParquetDecimal(20, 5)]
        public decimal Longitude { get; set; }
        // ----------------------------------------------------------------


        public static ParquetCustomer FromCustomer(DatabaseGenerator.Models.Customer c)
        {
            #pragma warning disable format
            return new ParquetCustomer()
            {
                CustomerKey   = c.CustomerID,
                GeoAreaKey    = c.GeoAreaID,
                StartDT       = c.StartDT,
                EndDT         = c.EndDT,
                Continent     = c.Continent,
                Gender        = c.Gender,
                Title         = c.Title,
                GivenName     = c.GivenName,
                MiddleInitial = c.MiddleInitial,
                Surname       = c.Surname,
                StreetAddress = c.StreetAddress,
                City          = c.City,
                State         = c.State,
                StateFull     = c.StateFull,
                ZipCode       = c.ZipCode,
                Country       = c.Country,
                CountryFull   = c.CountryFull,
                Birthday      = c.Birthday,
                Age           = c.Age,
                Occupation    = c.Occupation,
                Company       = c.Company,
                Vehicle       = c.Vehicle,
                Latitude      = (decimal)c.Latitude,
                Longitude     = (decimal)c.Longitude,
            };
            #pragma warning restore format
        }

        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("CustomerKey",   "integer",       false),
                        DeltaField.GetInstance("GeoAreaKey",    "integer",       false),
                        DeltaField.GetInstance("StartDT",       "timestamp",     true),
                        DeltaField.GetInstance("EndDT",         "timestamp",     true),
                        DeltaField.GetInstance("Continent",     "string",        true),
                        DeltaField.GetInstance("Gender",        "string",        true),
                        DeltaField.GetInstance("Title",         "string",        true),
                        DeltaField.GetInstance("GivenName",     "string",        true),
                        DeltaField.GetInstance("MiddleInitial", "string",        true),
                        DeltaField.GetInstance("Surname",       "string",        true),
                        DeltaField.GetInstance("StreetAddress", "string",        true),
                        DeltaField.GetInstance("City",          "string",        true),
                        DeltaField.GetInstance("State",         "string",        true),
                        DeltaField.GetInstance("StateFull",     "string",        true),
                        DeltaField.GetInstance("ZipCode",       "string",        true),
                        DeltaField.GetInstance("Country",       "string",        true),
                        DeltaField.GetInstance("CountryFull",   "string",        true),
                        DeltaField.GetInstance("Birthday",      "timestamp",     false),
                        DeltaField.GetInstance("Age",           "integer",       false),
                        DeltaField.GetInstance("Occupation",    "string",        true),
                        DeltaField.GetInstance("Company",       "string",        true),
                        DeltaField.GetInstance("Vehicle",       "string",        true),
                        DeltaField.GetInstance("Latitude",      "decimal(20,5)", false),
                        DeltaField.GetInstance("Longitude",     "decimal(20,5)", false),
                    }
            };
            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }

    }

}
