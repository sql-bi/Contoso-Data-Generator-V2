using System;


namespace DatabaseGenerator.Models
{
    public class GeoArea : WeightedItem
    {
        public int GeoAreaID { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string StateCode { get; set; }
        public string StateLongName { get; set; }
    }
}
