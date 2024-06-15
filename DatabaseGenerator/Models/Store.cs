using System;


namespace DatabaseGenerator.Models
{
    public class Store
    {
        public int StoreID { get; set; }
        public int GeoAreaID { get; set; }
        public string CountryCode { get; set; }   // filled from GeoArea "table"
        public DateTime? OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public int StoreCode { get; set; }
        public string Description { get; set; }
        public int? SquareMeters { get; set; }
        public string Status { get; set; }

        // -- added at runtime
        public string Country { get; set; }
        public string State { get; set; }
    }
}
