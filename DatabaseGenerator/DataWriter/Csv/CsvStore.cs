using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter.Csv
{

    internal class CsvStore
    {

        // ---------------------------------------------------------
        public int StoreKey { get; set; }
        public int StoreCode { get; set; }
        public int GeoAreaKey { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string Description { get; set; }
        public int? SquareMeters { get; set; }
        public string Status { get; set; }
        // ---------------------------------------------------------


        public static CsvStore GetFromStore(Store s)
        {
            return new CsvStore()
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

    }
}
