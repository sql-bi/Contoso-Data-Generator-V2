using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator.DataWriter.Csv
{
    public class CsvCustomer
    {
        public int CustomerKey { get; set; }
        public int GeoAreaKey { get; set; }
        public DateTime? StartDT { get; set; }
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
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public string Company { get; set; }
        public string Vehicle { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }


        public static CsvCustomer FromCustomer(Customer c)
        {
            return new CsvCustomer()
            {
                CustomerKey = c.CustomerID,
                GeoAreaKey = c.GeoAreaID,
                StartDT = c.StartDT,
                EndDT = c.EndDT,
                Continent = c.Continent,
                Gender = c.Gender,
                Title = c.Title,
                GivenName = c.GivenName,
                MiddleInitial = c.MiddleInitial,
                Surname = c.Surname,
                StreetAddress = c.StreetAddress,
                City = c.City,
                State = c.State,
                StateFull = c.StateFull,
                ZipCode = c.ZipCode,
                Country = c.Country,
                CountryFull = c.CountryFull,
                Birthday = c.Birthday,
                Age = c.Age,
                Occupation = c.Occupation,
                Company = c.Company,
                Vehicle = c.Vehicle,
                Latitude = (decimal)c.Latitude,
                Longitude = (decimal)c.Longitude,
            };
        }
    }
}
