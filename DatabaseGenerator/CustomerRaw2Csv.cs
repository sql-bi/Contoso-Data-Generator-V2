using CsvHelper;
using CsvHelper.Configuration;
using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator
{

    public class CustomerRAW2Csv
    {

        public class UKPostCode
        {
            public string Postcode { get; set; }
            public string Region { get; set; }
        }


        public static List<Customer> LoadCustomersFromGZCSV(string folderName, List<GeoArea> geoAreas)
        {
            var countryToContinent = new Dictionary<string, string>()
                                        {
                                            { "DE" , "Europe"        },
                                            { "GB" , "Europe"        },
                                            { "IT" , "Europe"        },
                                            { "NL" , "Europe"        },
                                            { "AU" , "Australia"     },
                                            { "CA" , "North America" },
                                            { "FR" , "Europe"        },
                                            { "US" , "North America" }
                                        };

            var ukPostCodesLookUp = new CsvReader(new StreamReader(Path.Combine(folderName, "UKPostcodes.csv")), CultureInfo.InvariantCulture)
                                       .GetRecords<UKPostCode>().ToDictionary(x => x.Postcode);

            var geoAreasLookUpDict = geoAreas.Where(x => x.CountryCode != null && x.StateCode != null)
                                             .ToDictionary(x => x.CountryCode.Trim() + x.StateCode.Trim());

            var allCustomers = new List<Customer>();

            int currentCustomerID = 0;

            Logger.Info($"csvgz FolderName: {folderName}");

            var csvGzFileList = Directory.GetFiles(folderName, "Cust.*.csv.gz").OrderBy(x => x).ToList();

            foreach (var csvGzFileName in csvGzFileList)
            {
                Logger.Info($"Processing: {Path.GetFileName(csvGzFileName)}");

                string csvBody = GzUnCompressToString(csvGzFileName);

                using (var csv = new CsvReader(new StringReader(csvBody), CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CustomerCSVGZMapping>();

                    var customers = csv.GetRecords<Customer>().ToList();

                    // Remove the not managed "US - Northern Mariana Islands" 
                    customers.RemoveAll(c => c.Country == "US" && c.State == "MP");

                    foreach (var cust in customers)
                    {
                        cust.CustomerID = currentCustomerID++;

                        // Correct City name if all uppercase
                        cust.City = NormalizeLettersCases(cust.City);

                        // update UK [State] from zip codes
                        if (cust.CountryFull == "United Kingdom")
                        {
                            string zipCodeFirstPart = cust.ZipCode.Split(' ')[0];
                            if (zipCodeFirstPart == "JE1") zipCodeFirstPart = "JE2";
                            if (zipCodeFirstPart == "CR1") zipCodeFirstPart = "CR0";
                            cust.State = cust.StateFull = ukPostCodesLookUp[zipCodeFirstPart].Region;
                        }

                        // fill [Continent]
                        cust.Continent = countryToContinent[cust.Country];

                        // fill [GeoAreaID]
                        var geoArea = geoAreasLookUpDict.GetValueOrDefault(cust.Country.Trim() + cust.State.Trim());
                        if (geoArea == null)
                            throw new Exception($"GeoArea lookup failed for [{cust.Country}-{cust.State}]");
                        cust.GeoAreaID = geoArea.GeoAreaID;
                    }

                    allCustomers.AddRange(customers);
                }
            }

            Logger.Info($"CustomerAll.csv - total items: {allCustomers.Count}");

            return allCustomers;
        }


        private static string GzUnCompressToString(string fileName)
        {
            using (var inStream = File.OpenRead(fileName))
            using (var outStream = new MemoryStream())
            {
                using (var gzip = new GZipStream(inStream, CompressionMode.Decompress, false))
                {
                    gzip.CopyTo(outStream);
                }
                var bytes = outStream.ToArray();

                return Encoding.GetEncoding(1252).GetString(outStream.ToArray());
            }
        }


        public class CustomerCSVGZMapping : ClassMap<Customer>
        {
            public CustomerCSVGZMapping()
            {
                Map(m => m.CustomerID).Name("Number");
                Map(m => m.Gender);
                Map(m => m.Title);
                Map(m => m.GivenName);
                Map(m => m.MiddleInitial);
                Map(m => m.Surname);
                Map(m => m.StreetAddress);
                Map(m => m.City);
                Map(m => m.State);
                Map(m => m.StateFull);
                Map(m => m.ZipCode);
                Map(m => m.Country);
                Map(m => m.CountryFull);
                Map(m => m.Birthday);
                Map(m => m.Age);
                Map(m => m.Occupation);
                Map(m => m.Company);
                Map(m => m.Vehicle);
                Map(m => m.Latitude);
                Map(m => m.Longitude);
            }
        }


        private static string NormalizeLettersCases(string s)
        {
            if (s.All(c => !Char.IsLetter(c) || (Char.IsLetter(c) && Char.IsUpper(c))))
            {
                var newName = new Char[s.Length];
                bool inWord = false;
                for (int i = 0; i < s.Length; i++)
                {
                    newName[i] = inWord ? Char.ToLower(s[i]) : s[i];                    
                    inWord = Char.IsLetter(s[i]);
                }
                s = new string(newName);
            }

            return s;
        }

    }

}
