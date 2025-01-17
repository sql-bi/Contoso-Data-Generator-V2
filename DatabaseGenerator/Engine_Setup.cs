using CsvHelper;
using DatabaseGenerator.Fast;
using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;


namespace DatabaseGenerator
{

    public partial class Engine
    {

        /// <summary>
        /// ...
        /// </summary>
        private void PrepareConfigData()
        {
            if (_config.CutDateBefore == null) _config.CutDateBefore = _config.StartDT;
            if (_config.CutDateAfter == null) _config.CutDateAfter = _config.StartDT.AddYears(_config.YearsCount);
        }


        /// <summary>
        /// ...
        /// </summary>
        private void PrepareWorkingFolder()
        {
            // Delete only known files and folders            

            Utility.DeleteFiles(_outputFolder, "*.csv", false);
            Utility.DeleteFiles(_outputFolder, "*.parquet", false);
            Utility.DeleteFiles(_outputFolder, "*.json", false);
            Utility.DeleteFiles(_outputFolder, "*.csv.gz", false);

            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.ORDERS), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.ORDERROWS), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.SALES), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.CURREXCHS), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.CUSTOMERS), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.DATES), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.PRODUCTS), true);
            Utility.DirectoryDeleteIfExists(Path.Combine(_outputFolder, Consts.STORES), true);
        }


        /// <summary>
        /// ...
        /// </summary>
        private void PrepareCustomersAllCSV()
        {
            // Do not change the order of the files!
            string[] namesCsvGzFileNames =
            {
                "Cust.AU.01.csv.gz",
                "Cust.AU.02.csv.gz",
                "Cust.CA.01.csv.gz",
                "Cust.CA.02.csv.gz",
                "Cust.DE.01.csv.gz",
                "Cust.DE.02.csv.gz",
                "Cust.FR.01.csv.gz",
                "Cust.IT.01.csv.gz",
                "Cust.NL.01.csv.gz",
                "Cust.UK.01.csv.gz",
                "Cust.UK.02.csv.gz",
                "Cust.UK.03.csv.gz",
                "Cust.US.01.csv.gz",
                "Cust.US.02.csv.gz",
                "Cust.US.03.csv.gz",
                "Cust.US.04.csv.gz",
                "Cust.US.05.csv.gz",
                "Cust.US.06.csv.gz",
                "Cust.US.07.csv.gz",
                "Cust.US.08.csv.gz",
                "Cust.US.09.csv.gz",
            };

            string ukPostalCodesCSV = "UKPostcodes.csv";

            // download files from GitHub
            var httpclient = new HttpClient();
            foreach (string fileName in namesCsvGzFileNames.Union(new[] { ukPostalCodesCSV }))
            {
                string url = Consts.DOWNLOAD_BASE_URL + fileName;
                string localFileFP = Path.Combine(_cacheFolder, fileName);

                if (!File.Exists(localFileFP))
                {
                    Logger.Info($"Downloading {url}");
                    byte[] body = httpclient.GetByteArrayAsync(url).Result;
                    File.WriteAllBytes(localFileFP, body);
                }
            }

            // create customersall.csv
            string customerAllCsvFileName = Path.GetFullPath(Path.Combine(_cacheFolder, "customersall.csv"));
            if (!File.Exists(customerAllCsvFileName))
            {
                Logger.Info("Customersall.csv not present. Create it.");
                var geoAreas = ExcelReader.Read<GeoArea>(Path.Combine(_dataFile), "geoareas");
                List<Customer> allCustomers = CustomerRAW2Csv.LoadCustomersFromGZCSV(_cacheFolder, geoAreas);
                using (var csv = new CsvWriter(new StreamWriter(customerAllCsvFileName), CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(allCustomers);
                }
            }
        }



        public void PrepareCurrencyExchangeFiles()
        {
            string[] fileNames = { Consts.FILE_ECB_EXCH_CSV };

            // download files from GitHub
            var httpclient = new HttpClient();
            foreach (string fileName in fileNames)
            {
                string url = Consts.DOWNLOAD_BASE_URL + fileName;
                string localFileFP = Path.Combine(_cacheFolder, fileName);

                if (!File.Exists(localFileFP))
                {
                    Logger.Info($"Downloading {url}");
                    byte[] body = httpclient.GetByteArrayAsync(url).Result;
                    File.WriteAllBytes(localFileFP, body);
                }
            }
        }



        /// <summary>
        /// ...
        /// </summary>
        private void ReadInputData(Random rng)
        {

            _categories = ExcelReader.Read<Category>(_dataFile, "categories");
            _subcategories = ExcelReader.Read<SubCategory>(_dataFile, "subcategories");
            _products = ExcelReader.Read<Product>(_dataFile, "products");
            _customerClusters = ExcelReader.Read<CustomerCluster>(_dataFile, "customerClusters");
            _geoAreas = ExcelReader.Read<GeoArea>(_dataFile, "geoareas");
            _stores = ExcelReader.Read<Store>(_dataFile, "stores");
            _subCatLinks = ExcelReader.Read<SubCategoryLink>(_dataFile, "subcatlinks");

            _currencyExchangeList = ECBExchangesReader.GetData(Path.Combine(_cacheFolder, Consts.FILE_ECB_EXCH_CSV), true);

            // Customers
            if (_config.CustomerFakeGenerator > 0)
            {
                Logger.Info("Generating fake customers");
                _customers = new List<Customer>();
                int geoAreasCount = _geoAreas.Count;
                for (int i = 0; i < _config.CustomerFakeGenerator; i++)
                {
                    _customers.Add(
                        new Customer()
                        {
                            CustomerID = i + 1,
                            GeoAreaID = _geoAreas[rng.Next(1, geoAreasCount)].GeoAreaID,
                            GivenName = Guid.NewGuid().ToString(),
                            Surname = Guid.NewGuid().ToString(),
                            Birthday = new DateTime(2000, 1, 1),
                            Age = 20
                        });
                }
            }
            else
            {
                Logger.Info("Loading customersall.csv");
                _customers = ReadCustomersFromCSVFile(Path.Combine(_cacheFolder, "customersall.csv"));

                // shuffle customers
                var localRnd = new Random(0);
                _customers = _customers.OrderBy(x => localRnd.Next()).ToList();
            }

            // get a percentage of customers
            Logger.Info($"TopN Customers - before : {_customers.Count}");
            int topN = (int)(_customers.Count * _config.CustomerPercentage);
            _customers = _customers.Take(topN).ToList();
            Logger.Info($"TopN Customers - after  : {_customers.Count}");
        }



        public List<Customer> ReadCustomersFromCSVFile(string customerAllCsvFileName)
        {
            using (var csv = new CsvReader(new StreamReader(customerAllCsvFileName), CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Customer>().ToList();
            }
        }



        private static List<DateExtended> GenerateDateExt(DateTime dt1, DateTime dt2)
        {
            CultureInfo ci = new CultureInfo("en-US");

            int workingDayNumber = 0;

            var dateExtItems = new List<DateExtended>();

            while (true)
            {
                int quarter = (dt1.Month - 1) / 3 + 1;
                string quarterStr = $"Q{quarter}";
                bool isHoliday = dt1.DayOfWeek == DayOfWeek.Saturday || dt1.DayOfWeek == DayOfWeek.Sunday || IsFederalHoliday(dt1);
                if (!isHoliday) workingDayNumber++;

                dateExtItems.Add(
                    new DateExtended()
                    {
                        Date = dt1,   //dt1.ToString("yyyy-MM-dd"),
                        DateKey = dt1.ToString("yyyyMMdd"),
                        Year = dt1.Year,
                        YearQuarter = $"{quarterStr}-{dt1.Year}",
                        YearQuarterNumber = (4 * dt1.Year + quarter),     // DATEPART(QUARTER,[Date]) + 4 * YEAR([Date]),
                        Quarter = quarterStr,
                        YearMonth = dt1.ToString("MMMM yyyy", ci),
                        YearMonthShort = dt1.ToString("MMM yyyy", ci),
                        YearMonthNumber = (dt1.Month + 12 * dt1.Year),    // DATEPART(MONTH,[Date]) + 12 * YEAR([Date])
                        Month = dt1.ToString("MMMM", ci),
                        MonthShort = dt1.ToString("MMM", ci),
                        MonthNumber = dt1.Month,
                        DayofWeek = dt1.ToString("dddd", ci),
                        DayofWeekShort = dt1.ToString("ddd", ci),
                        DayofWeekNumber = ((int)dt1.DayOfWeek + 1),//.ToString(),
                        WorkingDay = isHoliday ? 0 : 1,                    // working day
                        WorkingDayNumber = workingDayNumber   //.ToString()         // working day number
                    });

                dt1 = dt1.AddDays(1);
                if (dt1 > dt2) break;
            }

            return dateExtItems;
        }



        /// <summary>
        /// Determines if this date is a federal holiday.
        /// https://www.codeproject.com/Tips/1168428/US-Federal-Holidays-Csharp
        /// </summary>
        /// <param name="date">This date</param>
        /// <returns>True if this date is a federal holiday</returns>        
        public static bool IsFederalHoliday(DateTime date)
        {
            // to ease typing
            int nthWeekDay = (int)(Math.Ceiling((double)date.Day / 7.0d));
            DayOfWeek dayName = date.DayOfWeek;
            bool isThursday = dayName == DayOfWeek.Thursday;
            bool isFriday = dayName == DayOfWeek.Friday;
            bool isMonday = dayName == DayOfWeek.Monday;
            bool isWeekend = dayName == DayOfWeek.Saturday || dayName == DayOfWeek.Sunday;

            // New Years Day (Jan 1, or preceding Friday/following Monday if weekend)
            if ((date.Month == 12 && date.Day == 31 && isFriday) ||
                (date.Month == 1 && date.Day == 1 && !isWeekend) ||
                (date.Month == 1 && date.Day == 2 && isMonday)) return true;

            // MLK day (3rd monday in January)
            if (date.Month == 1 && isMonday && nthWeekDay == 3) return true;

            // President’s Day (3rd Monday in February)
            if (date.Month == 2 && isMonday && nthWeekDay == 3) return true;

            // Memorial Day (Last Monday in May)
            if (date.Month == 5 && isMonday && date.AddDays(7).Month == 6) return true;

            // Independence Day (July 4, or preceding Friday/following Monday if weekend)
            if ((date.Month == 7 && date.Day == 3 && isFriday) ||
                (date.Month == 7 && date.Day == 4 && !isWeekend) ||
                (date.Month == 7 && date.Day == 5 && isMonday)) return true;

            // Labor Day (1st Monday in September)
            if (date.Month == 9 && isMonday && nthWeekDay == 1) return true;

            // Columbus Day (2nd Monday in October)
            if (date.Month == 10 && isMonday && nthWeekDay == 2) return true;

            // Veteran’s Day (November 11, or preceding Friday/following Monday if weekend))
            if ((date.Month == 11 && date.Day == 10 && isFriday) ||
                (date.Month == 11 && date.Day == 11 && !isWeekend) ||
                (date.Month == 11 && date.Day == 12 && isMonday)) return true;

            // Thanksgiving Day (4th Thursday in November)
            if (date.Month == 11 && isThursday && nthWeekDay == 4) return true;

            // Christmas Day (December 25, or preceding Friday/following Monday if weekend))
            if ((date.Month == 12 && date.Day == 24 && isFriday) ||
                (date.Month == 12 && date.Day == 25 && !isWeekend) ||
                (date.Month == 12 && date.Day == 26 && isMonday)) return true;

            return false;
        }



        /// <summary>
        /// ...
        /// </summary>
        private void PrepareInputData(Random rng)
        {
            // fill "countrycode" in store
            FillStoreCountryCode();

            // setup "product fast"
            _productsFast = new ProductsFast() { Products = _products };
            _productsFast.IndexData();

            // calculate daily weights
            CalculateDailyWeights(_categories.ToArray());
            CalculateDailyWeights(_subcategories.ToArray());
            CalculateDailyWeights(_products.ToArray());
            CalculateDailyWeights(_geoAreas.ToArray());

            // add Start_date and End_date to customers
            AddStartEndDateToCustomers(rng);

            // setup "customer fast"
            _customersListFast = new CustomerListFast(_customers);

            // distribute customers across clusters
            DistributeCustomersAmongClusters(rng);

            // online probability distribution on each day
            _OnlineDailyPercentages = CalculateDailyWeights(_config.OnlinePerCent);

            // lambda for Poisson distr. of Delivery Date
            _DeliveryDateDayLambdas = CalculateDailyWeights(_config.DeliveryDateLambdaWeights);
        }


        private void FillStoreCountryCode()
        {
            foreach (var store in _stores)
            {
                var geoArea = _geoAreas.Single(x => x.GeoAreaID == store.GeoAreaID);
                store.CountryCode = geoArea.CountryCode;
            }
        }



        /// <summary>
        /// ...
        /// </summary>
        private void DistributeCustomersAmongClusters(Random rng)
        {
            // temp lists
            var tempCustomersPerCluster = new List<Customer>[_customerClusters.Count];
            for (int i = 0; i < _customerClusters.Count; i++)
            {
                tempCustomersPerCluster[i] = new List<Customer>();
            }

            // distribute
            double[] weights = _customerClusters.Select(cc => cc.CustomersWeight).ToArray();
            for (int i = 0; i < _customers.Count; i++)
            {
                int index = MyRnd.RandomIndexFromWeigthedDistribution(rng, weights);
                tempCustomersPerCluster[index].Add(_customers[i]);
            }

            // setup fast-search
            for (int i = 0; i < _customerClusters.Count; i++)
            {
                CustomerCluster cc = _customerClusters[i];
                cc.CustomersFast = new CustomerListFast(tempCustomersPerCluster[i]);
                Logger.Info($"Cluster: {cc.ClusterID}  ({cc.CustomersWeight}) --> {cc.CustomersFast.Count}");
            }
        }


        /// <summary>
        /// ...
        /// </summary>
        private void AddStartEndDateToCustomers(Random rng)
        {
            Logger.Info("AddStartEndDateToCustomers - START");

            DateTime startDT1 = new DateTime((int)_config.CustomerActivity.StartDateWeightPoints.First(), 1, 1);
            DateTime startDT2 = new DateTime((int)_config.CustomerActivity.StartDateWeightPoints.Last(), 12, 31);

            DateTime endDT1 = new DateTime((int)_config.CustomerActivity.EndDateWeightPoints.First(), 1, 1);
            DateTime endDT2 = new DateTime((int)_config.CustomerActivity.EndDateWeightPoints.Last(), 12, 31);

            double startDaysCount = startDT2.Subtract(startDT1).TotalDays;
            double endDaysCount = endDT2.Subtract(endDT1).TotalDays;

            var startDaysWeight = MyMath.Interpolate((int)startDaysCount,
                                                         _config.CustomerActivity.StartDateWeightPoints.First(),
                                                         _config.CustomerActivity.StartDateWeightPoints.Last(),
                                                         _config.CustomerActivity.StartDateWeightPoints,
                                                         _config.CustomerActivity.StartDateWeightValues);

            var endDaysWeight = MyMath.Interpolate((int)endDaysCount,
                                                         _config.CustomerActivity.EndDateWeightPoints.First(),
                                                         _config.CustomerActivity.EndDateWeightPoints.Last(),
                                                         _config.CustomerActivity.EndDateWeightPoints,
                                                         _config.CustomerActivity.EndDateWeightValues);

            DoublesArray startDaysWeight_Fast = new DoublesArray(startDaysWeight);
            DoublesArray endDaysWeight_Fast = new DoublesArray(endDaysWeight);

            int customerCount = _customers.Count;

            // to increase speed, process group of customers (1M --> groups of 10 customers)
            int step = customerCount / 100000;
            if (step == 0)
                step = 1;

            for (int i = 0; i < customerCount; i += step)
            {
                DateTime startDT = startDT1.AddDays(MyRnd.RandomIndexFromWeigthedDistribution(rng, startDaysWeight_Fast));
                DateTime endDT = endDT1.AddDays(MyRnd.RandomIndexFromWeigthedDistribution(rng, endDaysWeight_Fast));

                if (startDT > endDT)
                {
                    DateTime swapDT = startDT;
                    startDT = endDT;
                    endDT = swapDT;
                }

                for (int n = 0; n < step; n++)
                {
                    int j = i + n;

                    if (j < customerCount)
                    {
                        _customers[j].StartDT = startDT;
                        _customers[j].EndDT = endDT;
                    }
                }
            }

            Logger.Info("AddStartEndDateToCustomers - END");
        }


        /// <summary>
        /// ...
        /// </summary>
        private void CalculateDailyWeights(WeightedItem[] items)
        {
            foreach (var item in items)
            {
                item.DaysWeight = CalculateDailyWeights(item.Weights);
            }
        }


        /// <summary>
        /// ...
        /// </summary>
        private double[] CalculateDailyWeights(double[] weights)
        {
            int slicesCount = weights.Length;
            double[] points = MathNet.Numerics.Generate.LinearSpaced(slicesCount, 0, slicesCount);
            return MyMath.Interpolate(_totalDaysCount, 0, slicesCount, points, weights);
        }


        /// <summary>
        /// ...
        /// </summary>
        private void CalculateDaysWeight(int yearsCount, int daysCount, DateTime startDT)
        {
            if (_config.DaysWeight.DaysWeightConstant)
                _config.DaysWeight.DaysWeightValues = Enumerable.Repeat(1.0, _config.DaysWeight.DaysWeightPoints.Length).ToArray();

            // basic curve
            var daysWeight = MyMath.Interpolate(daysCount,
                                                _config.DaysWeight.DaysWeightPoints.First(),
                                                _config.DaysWeight.DaysWeightPoints.Last(),
                                                _config.DaysWeight.DaysWeightPoints,
                                                _config.DaysWeight.DaysWeightValues);

            // add some spike and low/high period
            if (_config.DaysWeight.DaysWeightAddSpikes)
            {

                // Annual spikes
                if (_config.AnnualSpikes != null)
                {
                    foreach (var annualSpike in _config.AnnualSpikes)
                    {
                        for (int y = 0; y < yearsCount; y++)
                        {
                            int arrayDayShift = (int)(new DateTime(startDT.Year + y, 1, 1).Subtract(startDT).TotalDays);

                            for (int day = annualSpike.StartDay; day < annualSpike.EndDay; day++)
                            {
                                double xFactor = 1 + (annualSpike.Factor - 1.0) * Math.Sin(Math.PI / (annualSpike.EndDay - annualSpike.StartDay) * (day - annualSpike.StartDay));
                                if (xFactor < 0) { xFactor = 0; }

                                int index = arrayDayShift + day;
                                if (index < daysWeight.Length)
                                {
                                    daysWeight[index] *= xFactor;
                                }
                            }
                        }
                    }
                }

                // OneTime spikes
                if (_config.OneTimeSpikes != null)
                {
                    // Calculate the multiplication factor.
                    // (days+1) and (i+1) required to create the spike also for a one-day-only spike.

                    foreach (var oneTimeSpike in _config.OneTimeSpikes)
                    {
                        int days = (int)oneTimeSpike.DT2.Subtract(oneTimeSpike.DT1).TotalDays + 1;
                        int baseShift = (int)oneTimeSpike.DT1.Subtract(startDT).TotalDays;

                        for (int i = 0; i < days; i++)
                        {
                            double xFactor = 1 + (oneTimeSpike.Factor - 1.0) * Math.Sin(Math.PI / (days + 1) * (i + 1));
                            if (xFactor < 0) { xFactor = 0; }

                            int index = baseShift + i;
                            if (index >= 0 && index < daysWeight.Length)
                            {
                                daysWeight[index] *= xFactor;
                            }
                        }
                    }
                }

                // Weekday weights 
                DateTime currentDay = startDT;
                for (int i = 0; i < daysWeight.Length; i++)
                {
                    daysWeight[i] *= _config.DaysWeight.WeekDaysFactor[(int)currentDay.DayOfWeek];
                    currentDay = currentDay.AddDays(1);
                }
            }

            _daysWeight = daysWeight;
        }


    }

}
