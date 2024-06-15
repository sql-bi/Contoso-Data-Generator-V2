using System;


namespace DatabaseGenerator.Models
{
    public class DateExtended
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
    }
}
