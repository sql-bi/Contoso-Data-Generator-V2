using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using DatabaseGenerator.DataWriter.Csv;
using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter
{

    public class CSVWriter : IDataWriter
    {

        // config
        private string _outputFolder;
        private bool _gzCompression;
        private int? _maxOrdersPerFile;
        private SOOutput _SOOutput;

        // running
        private int _filesCounter;
        private int _currentFileRowsCounter;
        private List<string> _ordersFileNames;
        private List<string> _orderRowsFileNames;
        private List<string> _salesFileNames;
        private CultureInfo _outputCultureInfo;
        private bool _fileOpen;
        private StreamWriter _orderFileStream;
        private StreamWriter _orderRowsFileStream;
        private StreamWriter _salesFileStream;



        public CSVWriter(Config config, string outputFolder)
        {
            _SOOutput = config.SalesOrdersOut;
            
            _maxOrdersPerFile = config.CsvMaxOrdersPerFile;
            if (_maxOrdersPerFile < 1) _maxOrdersPerFile = null;

            _gzCompression = config.CsvGzCompression == 1;
            _outputFolder = outputFolder;
            _outputCultureInfo = new CultureInfo(String.Empty);             // Creates an InvariantCulture that can be modified
            _outputCultureInfo.NumberFormat.NumberDecimalSeparator = ".";   // ...to be sure  :)
        }


        public void Init()
        {
            _filesCounter = 0;
            _fileOpen = false;
            _ordersFileNames = new List<string>();
            _orderRowsFileNames = new List<string>();
            _salesFileNames = new List<string>();
        }


        public async Task WriteOrderWithRows(Order order, IEnumerable<Sale> sales)
        {
            await Task.CompletedTask;

            // Create output file
            if (_fileOpen == false)
            {
                _fileOpen = true;

                if (_SOOutput.WriteOrders)
                {
                    string ordersFileName = _maxOrdersPerFile.HasValue ? $"{Consts.ORDERS}_{_filesCounter}.csv" : $"{Consts.ORDERS}.csv";
                    string orderRowsFileName = _maxOrdersPerFile.HasValue ? $"{Consts.ORDERROWS}_{_filesCounter}.csv" : $"{Consts.ORDERROWS}.csv";
                    string ordersFileFP = Path.Combine(_outputFolder, ordersFileName);
                    string orderRowsFileFP = Path.Combine(_outputFolder, orderRowsFileName);
                    _ordersFileNames.Add(ordersFileName);
                    _orderRowsFileNames.Add(orderRowsFileName);
                    _orderFileStream = new StreamWriter(ordersFileFP);
                    _orderRowsFileStream = new StreamWriter(orderRowsFileFP);
                    _orderFileStream.WriteLine(String.Join(",", DumpOrders_Headers()));
                    _orderRowsFileStream.WriteLine(String.Join(",", DumpOrderRows_Headers()));
                }

                if (_SOOutput.WriteSales)
                {
                    string salesFileName = _maxOrdersPerFile.HasValue ? $"{Consts.SALES}_{_filesCounter}.csv" : $"{Consts.SALES}.csv";
                    string salesFileFP = Path.Combine(_outputFolder, salesFileName);
                    _salesFileNames.Add(salesFileName);
                    _salesFileStream = new StreamWriter(salesFileFP);
                    //_salesFileStream = new StreamWriter(new FileStream(salesFileFP, FileMode.Create, FileAccess.Write, FileShare.None, 10 * 1024 * 1024));
                    _salesFileStream.WriteLine(String.Join(",", DumpSales_Headers()));
                }
            }

            // Write data to csv files
            if (_SOOutput.WriteOrders)
            {
                _orderFileStream.WriteLine(String.Join(",", DumpOrders_DataField(order)));
                foreach (var orderRow in order.Rows)
                {
                    _orderRowsFileStream.WriteLine(String.Join(",", DumpOrderRows_DataFields(order, orderRow, _outputCultureInfo)));
                }
            }
            if (_SOOutput.WriteSales)
            {
                foreach (var sale in sales)
                {
                    _salesFileStream.WriteLine(String.Join(",", DumpSale_DataField(sale, _outputCultureInfo)));
                }
            }

            // new file?
            _currentFileRowsCounter++;
            if (_maxOrdersPerFile.HasValue && _currentFileRowsCounter >= _maxOrdersPerFile)
            {
                if (_SOOutput.WriteOrders)
                {
                    _orderFileStream.Close();
                    _orderRowsFileStream.Close();
                    _orderFileStream = null;
                    _orderRowsFileStream = null;
                }

                if (_SOOutput.WriteSales)
                {
                    _salesFileStream.Close();
                    _salesFileStream = null;
                }

                _filesCounter++;
                _currentFileRowsCounter = 0;
                _fileOpen = false;
            }
        }


        public async Task WriteStaticData(IEnumerable<Customer> customers,
                                          IEnumerable<Store> stores,
                                          IEnumerable<Product> products,
                                          IEnumerable<DateExtended> dates,
                                          IEnumerable<CurrencyExchange> currencyExchanges)
        {
            Logger.Info("stores csv");
            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(_outputFolder, $"{Consts.STORES}.csv")), CultureInfo.InvariantCulture))
            {
                CsvWriterSetISODate(csv);
                await csv.WriteRecordsAsync(stores.Select(s => CsvStore.GetFromStore(s)));
            }

            Logger.Info("products csv");
            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(_outputFolder, $"{Consts.PRODUCTS}.csv")), CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(products.Select(p => CsvProduct.GetFromProduct(p)));
            }

            Logger.Info("dates csv");
            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(_outputFolder, $"{Consts.DATES}.csv")), CultureInfo.InvariantCulture))
            {
                CsvWriterSetISODate(csv);
                csv.WriteRecords(dates.Select(d => CsvDateExtended.FromDateExtended(d)));
            }

            Logger.Info("currency-exchanges csv");
            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(_outputFolder, $"{Consts.CURREXCHS}.csv")), CultureInfo.InvariantCulture))
            {
                CsvWriterSetISODate(csv);
                csv.WriteRecords(currencyExchanges.Select(ce => CsvCurrencyExchange.FromCurrencyExchange(ce)));
            }

            Logger.Info("customers csv");
            using (var csv = new CsvWriter(new StreamWriter(Path.Combine(_outputFolder, $"{Consts.CUSTOMERS}.csv")), CultureInfo.InvariantCulture))
            {
                CsvWriterSetISODate(csv);
                csv.WriteRecords(customers.Select(c => CsvCustomer.FromCustomer(c)).OrderBy(x => x.CustomerKey));
            }
        }


        private void CsvWriterSetISODate(CsvWriter csv)
        {
            var tcoDT = new TypeConverterOptions() { Formats = new[] { "yyyy-MM-dd" } };
            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(tcoDT);
            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(tcoDT);
        }


        public void Close()
        {
            if (_fileOpen)
            {
                if (_SOOutput.WriteOrders) _orderFileStream.Close();
                if (_SOOutput.WriteOrders) _orderRowsFileStream.Close();
                if (_SOOutput.WriteSales) _salesFileStream.Close();
            }

            if (_maxOrdersPerFile.HasValue)
            {
                Logger.Info($"Move csv files to subfoldes");

                string ordersFolder = Path.Combine(_outputFolder, Consts.ORDERS);
                string orderRowsFolder = Path.Combine(_outputFolder, Consts.ORDERROWS);
                string salesFolder = Path.Combine(_outputFolder, Consts.SALES);

                if (_SOOutput.WriteOrders) Directory.CreateDirectory(ordersFolder);
                if (_SOOutput.WriteOrders) Directory.CreateDirectory(orderRowsFolder);
                if (_SOOutput.WriteSales) Directory.CreateDirectory(salesFolder);

                if (_SOOutput.WriteOrders) _ordersFileNames.ForEach(fn => MoveAndGZ(fn, ordersFolder));
                if (_SOOutput.WriteOrders) _orderRowsFileNames.ForEach(fn => MoveAndGZ(fn, orderRowsFolder));
                if (_SOOutput.WriteSales) _salesFileNames.ForEach(fn => MoveAndGZ(fn, salesFolder));
            }
        }


        private void MoveAndGZ(string fileName, string destFolder)
        {
            string srcFile = Path.Combine(_outputFolder, fileName);
            string dstFile = Path.Combine(destFolder, fileName);
            string gzFile = Path.Combine(destFolder, fileName + ".gz");

            File.Move(srcFile, dstFile);

            if (_gzCompression)
            {
                GzCompress(dstFile, gzFile, true);
            }
        }


        private void GzCompress(string inFileName, string gzFileName, bool deleteOriginal)
        {
            using (var inStream = File.OpenRead(inFileName))
            using (var outStream = File.OpenWrite(gzFileName))
            {
                using (var gzip = new GZipStream(outStream, CompressionMode.Compress, false))
                {
                    inStream.CopyTo(gzip);
                }
            }

            if (deleteOriginal) File.Delete(inFileName);
        }


        public string[] DumpOrders_Headers()
        {
            return new string[]
            {
                "OrderKey",
                "CustomerKey",
                "StoreKey",
                "OrderDate",
                "DeliveryDate",
                "CurrencyCode"
            };
        }


        public string[] DumpOrders_DataField(Order order)
        {
            return new string[]
            {
                order.OrderID.ToString(),
                order.CustomerID.ToString(),
                order.StoreID.ToString(),
                order.DT.ToString("yyyy-MM-dd"),
                order.DeliveryDate.ToString("yyyy-MM-dd"),
                order.CurrencyCode
            };
        }


        private string[] DumpOrderRows_Headers()
        {
            return new string[]
            {
                "OrderKey",
                "LineNumber",
                "ProductKey",
                "Quantity",
                "UnitPrice",
                "NetPrice",
                "UnitCost"
            };
        }


        private string[] DumpOrderRows_DataFields(Order order, OrderRow orderRow, CultureInfo ci)
        {
            return new string[]
            {
                order.OrderID.ToString(),
                orderRow.RowNumber.ToString(),
                orderRow.ProductID.ToString(),
                orderRow.Quantity.ToString(),
                orderRow.UnitPrice.ToString(ci),
                orderRow.NetPrice.ToString(ci),
                orderRow.UnitCost.ToString(ci)
            };
        }

        public string[] DumpSales_Headers()
        {
            return new string[]
            {
                "OrderKey",
                "LineNumber",
                "OrderDate",
                "DeliveryDate",
                "CustomerKey",
                "StoreKey",
                "ProductKey",
                "Quantity",
                "UnitPrice",
                "NetPrice",
                "UnitCost",
                "CurrencyCode",
                "ExchangeRate"
            };
        }

        public string[] DumpSale_DataField(Sale sale, CultureInfo ci)
        {
            return new string[]
            {
                sale.OrderKey.ToString(),
                sale.LineNumber.ToString(),
                sale.OrderDate.ToString("yyyy-MM-dd"),
                sale.DeliveryDate.ToString("yyyy-MM-dd"),
                sale.CustomerKey.ToString(),
                sale.StoreKey.ToString(),
                sale.ProductKey.ToString(),
                sale.Quantity.ToString(),
                sale.UnitPrice.ToString(ci),
                sale.NetPrice.ToString(ci),
                sale.UnitCost.ToString(ci),
                sale.CurrencyCode,
                sale.ExchangeRate.ToString(ci)
            };
        }

    }

}
