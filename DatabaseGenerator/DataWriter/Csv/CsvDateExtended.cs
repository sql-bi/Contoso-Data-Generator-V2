using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator.DataWriter.Csv
{
    internal class CsvDateExtended
    {
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


        public static CsvDateExtended FromDateExtended(DateExtended de)
        {
            return new CsvDateExtended()
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

    }
}
