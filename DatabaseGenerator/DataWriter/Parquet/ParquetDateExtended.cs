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
    public class ParquetDateExtended
    {

        // ----------------------------------------------------------------
        [ParquetTimestamp(ParquetTimestampResolution.Milliseconds)]
        public DateTime Date { get; set; }
        public string DateKey { get; set; }
        public int Year { get; set; }
        public string YearQuarter { get; set; }
        public int YearQuarterNumber { get; set; }
        public string Quarter { get; set; }
        public string YearMonth { get; set; }
        public string YearMonthShort { get; set; }
        public int YearMonthNumber { get; set; }
        public string Month { get; set; }
        public string MonthShort { get; set; }
        public int MonthNumber { get; set; }
        public string DayofWeek { get; set; }
        public string DayofWeekShort { get; set; }
        public int DayofWeekNumber { get; set; }
        public int WorkingDay { get; set; }
        public int WorkingDayNumber { get; set; }
        // ----------------------------------------------------------------



        public static ParquetDateExtended FromDateExtended(DateExtended de)
        {
            return new ParquetDateExtended()
            {
                Date = de.Date,
                DateKey = de.DateKey,
                Year = de.Year,
                YearQuarter = de.YearQuarter,
                YearQuarterNumber = de.YearQuarterNumber,
                Quarter = de.Quarter,
                YearMonth = de.YearMonth,
                YearMonthShort = de.YearMonthShort,
                YearMonthNumber = de.YearMonthNumber,
                Month = de.Month,
                MonthShort = de.MonthShort,
                MonthNumber = de.MonthNumber,
                DayofWeek = de.DayofWeek,
                DayofWeekShort = de.DayofWeekShort,
                DayofWeekNumber = de.DayofWeekNumber,
                WorkingDay = de.WorkingDay,
                WorkingDayNumber = de.WorkingDayNumber,
            };
        }




        public static string GETDeltaSchema()
        {
            var schema = new DeltaSchema()
            {
                type = "struct",
                fields = new List<DeltaField>()
                    {
                        DeltaField.GetInstance("Date",          "timestamp",       false),
                        DeltaField.GetInstance("DateKey",       "string",        true),
                        DeltaField.GetInstance("Year",           "integer",        false),
                        DeltaField.GetInstance("YearQuarter",     "string",        true),
                        DeltaField.GetInstance("YearQuarterNumber",            "integer",        false),
                        DeltaField.GetInstance("Quarter",            "string",        true),
                        DeltaField.GetInstance("YearMonth",       "string",        true),
                        DeltaField.GetInstance("YearMonthShort",   "string", true),
                        DeltaField.GetInstance("YearMonthNumber",     "integer", false),
                        DeltaField.GetInstance("Month",            "string", true),
                        DeltaField.GetInstance("MonthShort",       "string",       true),
                        DeltaField.GetInstance("MonthNumber",      "integer",        false),
                        DeltaField.GetInstance("DayofWeek",          "string",       false),
                        DeltaField.GetInstance("DayofWeekShort",     "string",        true),
                        DeltaField.GetInstance("DayofWeekNumber",    "integer",        false),
                        DeltaField.GetInstance("WorkingDay",         "integer",        false),
                        DeltaField.GetInstance("WorkingDayNumber",   "integer",        false),
                    }
            };

            return JsonSerializer.Serialize(schema, DeltaSchemaSerializerContext.Default.DeltaSchema);
        }
    }
}
