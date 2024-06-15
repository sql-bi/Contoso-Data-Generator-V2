using DatabaseGenerator.DataWriter;
using DatabaseGenerator.Fast;
using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator
{

    public partial class Engine
    {

        private const string LOG_FILENAME = "_log.log";


        private Config _config;
        private string _dataFile;
        private int _totalDaysCount;
        private long _orderNumberDailyRange;

        private string _outputFolder;
        private string _cacheFolder;

        private CultureInfo _outputCultureInfo;


        private double[] _daysWeight;
        private List<Category> _categories;
        private List<SubCategory> _subcategories;
        private List<Product> _products;
        private List<GeoArea> _geoAreas;
        private List<CustomerCluster> _customerClusters;
        private List<Customer> _customers;
        private CustomerListFast _customersListFast;
        private ProductsFast _productsFast;
        private List<Store> _stores;
        private List<SubCategoryLink> _subCatLinks;
        private double[] _OnlineDailyPercentages;
        private double[] _DeliveryDateDayLambdas;

        private List<CurrencyExchange> _currencyExchangeList;

        private int _CounterCannotAssignStore = 0;
        private int _CounterOnlineStore = 0;


        /// <summary>
        /// ...
        /// </summary>
        public Engine(string datafile, string outputFolder, string cacheFolder, Config config)
        {
            _dataFile = datafile;
            _outputFolder = outputFolder;
            _cacheFolder = cacheFolder;
            _config = config;

            if (!File.Exists(_dataFile)) throw new Exception($"DataFile does not exist [{_dataFile}]");
            if (!Directory.Exists(_cacheFolder)) Directory.CreateDirectory(_cacheFolder);
            if (!Directory.Exists(_outputFolder)) Directory.CreateDirectory(_outputFolder);

            _totalDaysCount = (int)(_config.StartDT.AddYears(_config.YearsCount).Subtract(_config.StartDT).TotalDays);
            _orderNumberDailyRange = (long)Math.Pow(10, Math.Ceiling(Math.Log10(_config.OrdersCount / _totalDaysCount)) + 1);

            _outputCultureInfo = new CultureInfo(String.Empty);             // Creates an InvariantCulture that can be modified
            _outputCultureInfo.NumberFormat.NumberDecimalSeparator = ".";   // ...to be sure  :)
        }


        /// <summary>
        /// ...
        /// </summary>
        public async Task Exec()
        {
            DateTime startDT = DateTime.UtcNow;

            // - RandomNumberGenerator -
            // DO NOT CHANGE SEED !!!
            // Do not not use a static global rng: it's not supported in multithread
            Random rng = new Random(0);

            Logger.Init(Path.Combine(_outputFolder, LOG_FILENAME));

            try
            {
                var exeAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var productName = exeAssembly
                    .GetCustomAttributes(typeof(System.Reflection.AssemblyProductAttribute), false)
                    .Cast<System.Reflection.AssemblyProductAttribute>()
                    .FirstOrDefault()
                    ?.Product;

                Logger.Info("START");
                Logger.Info($"Ver: {exeAssembly.GetName().Version}");
                Logger.Info($"Info: {productName}");
                Logger.Info($"DataFile     : {_dataFile}");
                Logger.Info($"OutputFolder : {_outputFolder}");
                Logger.Info($"OutputFormat : {_config.OutputFormat}");

                // some calculation on config data
                PrepareConfigData();

                Logger.Info($"StartDT      : {_config.StartDT}");
                Logger.Info($"TotalDays    : {_totalDaysCount}");
                Logger.Info($"O_N_D_Range  : {_orderNumberDailyRange}");
                Logger.Info($"CutDateBefore: {_config.CutDateBefore}");
                Logger.Info($"CutDateAfter : {_config.CutDateAfter}");
                Logger.Info($"OrdersCount  : {_config.OrdersCount}");

                // delete existing output files
                PrepareWorkingFolder();

                // Read data from excel file & other sources
                Logger.Info($"Read input data");
                PrepareCustomersAllCSV();
                PrepareCurrencyExchangeFiles();
                ReadInputData(rng);
                Logger.Info($"CustomersCount : {_customers.Count}");

                // Pre-calculate daily weights for categories, subcategories, etc... and other data
                Logger.Info($"Prepare input data");
                PrepareInputData(rng);

                // Days weights
                CalculateDaysWeight(_config.YearsCount, _totalDaysCount, _config.StartDT);

                // Create Orders and Order-Rows
                int logOrderCounter = 0;
                int logOrderRowCounter = 0;

                // Setup data writer
                IDataWriter dataWriter;
                switch (_config.OutputFormat)
                {
                    case "CSV": dataWriter = new CSVWriter(_config, _outputFolder); break;
                    case "PARQUET": dataWriter = new ParquetWriter(_config, _outputFolder); break;
                    case "DELTATABLE": dataWriter = new ParquetWriter(_config, _outputFolder); break;
                    default: throw new NotSupportedException($"Unknown output format [{_config.OutputFormat}]");
                }

                //Currency exchange lookup dictionary
                var currExchLookupDict = new Dictionary<long, Dictionary<string, CurrencyExchange>>();
                foreach (var item in _currencyExchangeList.Where(x => x.FromCurrency == "USD"))
                {
                    if (!currExchLookupDict.ContainsKey(item.Date.Ticks))
                    {
                        currExchLookupDict.Add(item.Date.Ticks, new Dictionary<string, CurrencyExchange>());
                    }
                    currExchLookupDict[item.Date.Ticks].Add(item.ToCurrency, item);
                }

                GC.Collect();

                // data output
                DateTime ordersCycleStartDT = DateTime.UtcNow;
                DateTime? orderMinDate = null;
                DateTime orderMaxDeliveryDate = DateTime.MinValue;
                dataWriter.Init();

                int parallelFactor = 10;

                var histogramData = new int[_totalDaysCount];
                var swData = new System.Diagnostics.Stopwatch();
                var swFile = new System.Diagnostics.Stopwatch();

                for (int dayNumber = 0; dayNumber < _totalDaysCount; dayNumber = dayNumber + parallelFactor)
                {
                    if (logOrderCounter > 1000)
                    {
                        TimeSpan toTheEnd = (DateTime.UtcNow.Subtract(ordersCycleStartDT) / logOrderCounter) * (_config.OrdersCount - logOrderCounter);
                        Logger.Info($"{100 * dayNumber / _totalDaysCount}%  Orders: {logOrderCounter} - [{toTheEnd.ToString("hh\\:mm\\:ss")}]");
                    }

                    // -- Create data on parallel runs and merge it

                    swData.Start();

                    List<Order>[] tempParallelData = new List<Order>[parallelFactor];
                    Parallel.For(0, parallelFactor, (i) =>
                        {
                            int shift = i;
                            if (dayNumber + shift < _totalDaysCount)
                            {
                                tempParallelData[shift] = CreateDayOrders(dayNumber + shift);
                                histogramData[dayNumber + shift] = tempParallelData[shift].Count;
                            }
                            else
                            {
                                tempParallelData[shift] = null;
                            }
                        });

                    List<Order> multiDaysOrders = new List<Order>();
                    for (int i = 0; i < parallelFactor; i++)
                    {
                        if (tempParallelData[i] != null)
                        {
                            multiDaysOrders.AddRange(tempParallelData[i]);
                        }
                    }

                    swData.Stop();

                    // -- Output data to file
                    swFile.Start();
                    foreach (var order in multiDaysOrders.Where(x => x.DT >= _config.CutDateBefore && x.DT <= _config.CutDateAfter))
                    {
                        decimal exchangeRate = currExchLookupDict[order.DT.Ticks][order.CurrencyCode].Exchange;
                        var sales = BuildSaleItems(order, exchangeRate);
                        await dataWriter.WriteOrderWithRows(order, sales);
                        logOrderCounter++;
                        logOrderRowCounter += order.Rows.Count;
                        if (orderMinDate == null) orderMinDate = order.DT;
                        if (order.DeliveryDate > orderMaxDeliveryDate) orderMaxDeliveryDate = order.DeliveryDate;
                    }
                    swFile.Stop();                    
                }

                Logger.Info($"Sales processing: data={swData.Elapsed} files={swFile.Elapsed}");

                {
                    DateTime dtStartFirstYear = new DateTime(orderMinDate.Value.Year, 1, 1);
                    DateTime dtEndLastYear = new DateTime(orderMaxDeliveryDate.Year, 12, 31);

                    _currencyExchangeList = _currencyExchangeList.Where(x => x.Date >= dtStartFirstYear && x.Date <= dtEndLastYear).ToList();
                    List<DateExtended> dateExtList = GenerateDateExt(dtStartFirstYear, dtEndLastYear);
                    Converter2OUT.EnrichStores(_stores, _geoAreas);
                    Converter2OUT.EnrichProducts(_products, _categories, _subcategories);

                    await dataWriter.WriteStaticData(_customers, _stores, _products, dateExtList, _currencyExchangeList);
                }

                dataWriter.Close();

                DumpHistogram(histogramData);

                Logger.Info($"Orders:            {logOrderCounter}");
                Logger.Info($"Online orders:     {_CounterOnlineStore}");
                Logger.Info($"OrdersRows:        {logOrderRowCounter}");
                Logger.Info($"CannotAssignStore: {_CounterCannotAssignStore}");
                Logger.Info($"Elapsed:           {DateTime.UtcNow.Subtract(startDT).TotalSeconds.ToString("0.000")}");
                Logger.Info("THE END");

            }
            catch (Exception ex)
            {
                Logger.Info($"EXCEPTION: {ex.ToString()}");
                throw;
            }
        }


        private IEnumerable<Sale> BuildSaleItems(Order order, decimal er)
        {
            foreach (var orderRow in order.Rows)
            {
                yield return new Sale()
                {
                    OrderKey = order.OrderID,
                    LineNumber = orderRow.RowNumber,
                    OrderDate = order.DT,
                    DeliveryDate = order.DeliveryDate,
                    CustomerKey = order.CustomerID,
                    StoreKey = order.StoreID,
                    ProductKey = orderRow.ProductID,
                    Quantity = orderRow.Quantity,
                    UnitPrice = orderRow.UnitPrice,
                    NetPrice = orderRow.NetPrice,
                    UnitCost = orderRow.UnitCost,
                    CurrencyCode = order.CurrencyCode,
                    ExchangeRate = er
                };
            }
        }



        /// <summary>
        /// ...
        /// </summary>
        private List<Order> CreateDayOrders(int dayNumber)
        {
            var today = _config.StartDT.AddDays(dayNumber);
            var localRNG = new Random(today.Year * 1000 + today.DayOfYear);
            int transactionsToday = (int)((double)_config.OrdersCount * _daysWeight[dayNumber] / _daysWeight.Sum());
            transactionsToday = localRNG.Next((int)(transactionsToday * (1.0 - _config.DaysWeight.DayRandomness)),
                                              (int)(transactionsToday * (1.0 + _config.DaysWeight.DayRandomness)));

            var todayOrders = new List<Order>();

            for (int i = 0; i < transactionsToday; i++)
            {
                long orderNumber = (dayNumber + 1) * _orderNumberDailyRange + i;

                var order = BuildOrder(orderNumber, today, dayNumber, localRNG);

                if (order != null)
                {
                    todayOrders.Add(order);
                }
                else
                {
                    //nullCounter++;
                }
            }

            return todayOrders;
        }


        /// <summary>
        /// ...
        /// </summary>
        private Order BuildOrder(long orderNumber, DateTime dt, int dayNumber, Random RNG)
        {

            // ------------------------------------------
            // CustomerCluster
            CustomerCluster customerCluster;
            {
                double[] ccOrderWeights = _customerClusters.Select(cc => cc.OrdersWeight).ToArray();
                var index = MyRnd.RandomIndexFromWeigthedDistribution(RNG, ccOrderWeights);
                customerCluster = _customerClusters[index];
            }

            // ------------------------------------------
            // GeoArea
            GeoArea geoArea;
            {
                double[] weights = _geoAreas.Select(x => x.DaysWeight[dayNumber]).ToArray();
                int index = MyRnd.RandomIndexFromWeigthedDistribution(RNG, weights);
                geoArea = _geoAreas[index];
            }

            // ------------------------------------------
            // Customer
            Customer customer = null;
            {
                var customerSubGroup = customerCluster.CustomersFast.FindByGeoAreaID(geoArea.GeoAreaID);

                // OLD (too slow)
                //customerSubGroup = customerSubGroup.Where(c => dt > c.StartDT && dt < c.EndDT).ToList();
                //if (customerSubGroup.Count == 0)
                //    return null;   // <--- exit ???
                //customer = customerSubGroup[RNG.Next(customerSubGroup.Count)];

                // NEW  (this is a lot faster, although is less precise)
                //      Random pick of customers from subgroup
                if (customerSubGroup.Count > 0)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        var tempCustomer = customerSubGroup[RNG.Next(customerSubGroup.Count)];
                        if (dt > tempCustomer.StartDT && dt < tempCustomer.EndDT)
                        {
                            customer = tempCustomer;
                            break;
                        }
                    }
                }

                if (customer == null)
                {
                    return null;
                }
            }

            // ------------------------------------------
            // Store
            Store store = null;
            {
                double todayOnlinePercent = _OnlineDailyPercentages[dayNumber];

                if (RNG.NextDouble() < (1.0 - todayOnlinePercent))
                {
                    // Try to find an open store in the same GeoArea
                    var storesAvailable = _stores
                        .Where(s => s.GeoAreaID == customer.GeoAreaID)
                        .Where(s => s.OpenDate == null || s.OpenDate.Value <= dt)
                        .Where(s => s.CloseDate == null || s.CloseDate >= dt)
                        .ToList();

                    if (storesAvailable.Count > 0)
                    {
                        store = storesAvailable[RNG.Next(storesAvailable.Count)];
                    }
                    else
                    {
                        // Try to find an open store in the same CountryCode
                        List<Store> alternativeStores = _stores
                                        .Where(x => x.CountryCode == geoArea.CountryCode)
                                        .Where(s => s.OpenDate == null || s.OpenDate.Value <= dt)
                                        .Where(s => s.CloseDate == null || s.CloseDate >= dt)
                                        .ToList();

                        if (alternativeStores.Count > 0)
                        {
                            store = alternativeStores[RNG.Next(alternativeStores.Count)];
                        }
                        else
                        {
                            // do nothing
                            _CounterCannotAssignStore++;
                        }
                    }
                }

                if (store == null)
                {
                    store = _stores.Single(s => s.GeoAreaID == -1);  // online store
                    _CounterOnlineStore++;
                }
            }

            // ------------------------------------------
            // Order
            var order = new Order()
            {
                OrderID = orderNumber,
                CustomerID = customer.CustomerID,
                StoreID = store.StoreID,
                DT = dt,
                DeliveryDate = dt,
                CurrencyCode = _config.CountryCurrency[geoArea.CountryCode],
                Rows = new List<OrderRow>()
            };

            // recalculate delivery date for online orders
            if (store.GeoAreaID == -1)
            {
                var deltaDays = 1 + MathNet.Numerics.Distributions.Poisson.Sample(RNG, _DeliveryDateDayLambdas[dayNumber]);
                order.DeliveryDate = dt.AddDays(deltaDays);
            }

            // ------------------------------------------
            // Order-Rows
            int numberOfRows = 1 + MyRnd.RandomIndexFromWeigthedDistribution(RNG, _config.OrderRowsWeights);
            for (int rowNumber = 0; rowNumber < numberOfRows; rowNumber++)
            {
                var orderRow = BuildOrderRow(rowNumber, dayNumber, null, RNG);
                order.Rows.Add(orderRow);

                // add related products?
                var linkedSubCategoris = _subCatLinks.Where(sc => sc.SubCategoryID == orderRow.SubCategoryID).ToList();
                foreach (var linkedSubCat in linkedSubCategoris)
                {
                    if (RNG.NextDouble() < linkedSubCat.PerCent)
                    {
                        // add a related product
                        rowNumber++;
                        var orderRow2 = BuildOrderRow(rowNumber, dayNumber, linkedSubCat.LinkedSubCategoryID, RNG);
                    }
                }
            }

            return order;
        }


        /// <summary>
        /// ...
        /// </summary>
        private OrderRow BuildOrderRow(int rowNumber, int dayNumber, int? fixedSubcategory, Random RNG)
        {
            Category category = null;
            SubCategory subCategory = null;

            if (fixedSubcategory.HasValue)
            {
                subCategory = _subcategories.Single(sc => sc.SubCategoryID == fixedSubcategory);
                category = _categories.Single(c => c.CategoryID == subCategory.CategoryID);
            }

            // ------------------------------------------
            // Category
            if (category == null)
            {
                double[] weights = _categories.Select(cat => cat.DaysWeight[dayNumber]).ToArray();
                int index = MyRnd.RandomIndexFromWeigthedDistribution(RNG, weights);
                category = _categories[index];
            }

            // ------------------------------------------
            // Subcatecory
            if (subCategory == null)
            {
                var availableSubCategories = _subcategories.Where(x => x.CategoryID == category.CategoryID).ToList();
                double[] subcategories_today_weights = availableSubCategories.Select(sc => sc.DaysWeight[dayNumber]).ToArray();
                int index = MyRnd.RandomIndexFromWeigthedDistribution(RNG, subcategories_today_weights);
                subCategory = availableSubCategories[index];
            }

            // ------------------------------------------
            // Product
            Product product;
            {
                var availableProcuts = _productsFast.GetProductsBySubCategoryID(subCategory.SubCategoryID);
                double[] products_today_weights = availableProcuts.Select(p => p.DaysWeight[dayNumber]).ToArray();
                int index = MyRnd.RandomIndexFromWeigthedDistribution(RNG, products_today_weights);
                product = availableProcuts[index];
            }

            // ------------------------------------------
            // Quantity
            int qty = 1 + MyRnd.RandomIndexFromWeigthedDistribution(RNG, _config.OrderQuantityWeights);

            // ------------------------------------------
            // Price and discount
            double unitPrice = product.Price * category.PricePerCent[category.PricePerCent.Length * dayNumber / _totalDaysCount];
            double unitCost = product.Cost * category.PricePerCent[category.PricePerCent.Length * dayNumber / _totalDaysCount];

            double discount = (double)MyRnd.RandomIndexFromWeigthedDistribution(RNG, _config.DiscountWeights);
            double netPrice = unitPrice * (1.0 - discount / 100.0);

            // Create the row
            var orderRow = new OrderRow()
            {
                RowNumber = rowNumber,
                Quantity = qty,
                CategoryID = category.CategoryID,
                SubCategoryID = subCategory.SubCategoryID,
                ProductID = product.ProductID,
                UnitPrice = (decimal)unitPrice,
                NetPrice = (decimal)netPrice,
                UnitCost = (decimal)unitCost,
            };

            return orderRow;
        }


        private void DumpHistogram(int[] histogramData)
        {
            int widthChars = 100;
            int maxHeightChars = 20;
            int sampling = (int)Math.Ceiling(histogramData.Length / (double)widthChars);

            int counter = 0;
            var histogramDataGrouped = histogramData
                                        .Select(x => { counter++; return new { counter, x }; })
                                        .GroupBy(item => item.counter / sampling)
                                        .Select(item => item.Sum(a => a.x))
                                        .ToList();

            int istogramMax = histogramDataGrouped.Max();

            histogramDataGrouped = histogramDataGrouped.Select(x => x * maxHeightChars / istogramMax).ToList();

            Logger.Info(new string('-', histogramDataGrouped.Count));

            for (int i = maxHeightChars; i >= 0; i--)
            {
                var sb = new StringBuilder();
                foreach (var x in histogramDataGrouped)
                {
                    sb.Append(x > i ? "*" : " ");
                }
                Logger.Info(sb.ToString());
            }

            Logger.Info(new string('-', histogramDataGrouped.Count));
        }

    }

}
