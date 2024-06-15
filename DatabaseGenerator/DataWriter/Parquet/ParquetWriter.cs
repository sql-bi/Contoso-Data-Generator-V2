using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Parquet.Serialization;
using DatabaseGenerator.DataWriter.Parquet;
using ParquetLib = Parquet;


namespace DatabaseGenerator.DataWriter
{

    public class ParquetWriter : IDataWriter
    {
        
        // config
        private string _outputFolder;
        private bool _isDeltaTableOutput;
        private long _maxOrdersPerFile;
        private int _parquetOrdersRowGroupSize;
        private SOOutput _SOOutput;

        // running
        private int _fileCounter;                 // counter of generated files
        private int _rowGroupCounter;             // counter of generated parquet "row groups"
        private int _currentFileOrderCounter;     // number of orders of the current file        
        private bool _appendToCurrentFile;        // "false" if the current is new
        private List<Order> _memoryBufferOrders;
        private List<Sale> _memoryBufferSales;
        private List<string> _ordersFiles;
        private List<string> _orderRowsFiles;
        private List<string> _salesFiles;

        // logging
        private void Log(string message) { Logger.Info($"Parquet : {message}"); }



        public ParquetWriter(Config config, string outputFolder)
        {
            _SOOutput = config.SalesOrdersOut;

            if (config.DeltaTableOrdersPerFile == 0) config.DeltaTableOrdersPerFile = int.MaxValue;

            _isDeltaTableOutput = (config.OutputFormat == "DELTATABLE");
            _outputFolder = outputFolder;
            _maxOrdersPerFile = _isDeltaTableOutput ? config.DeltaTableOrdersPerFile : long.MaxValue;
            _parquetOrdersRowGroupSize = config.ParquetOrdersRowGroupSize;

            if (_parquetOrdersRowGroupSize < 1) _parquetOrdersRowGroupSize = 500 * 1000;
        }


        public void Init()
        {
            _fileCounter = 0;
            _currentFileOrderCounter = 0;
            _appendToCurrentFile = false;
            _memoryBufferOrders = new List<Order>();
            _memoryBufferSales = new List<Sale>();
            _ordersFiles = new List<string>();
            _orderRowsFiles = new List<string>();
            _salesFiles = new List<string>();
        }


        public async Task WriteOrderWithRows(Order order, IEnumerable<Sale> sales)
        {
            await Task.CompletedTask;

            _memoryBufferOrders.Add(order);
            _memoryBufferSales.AddRange(sales);
            _currentFileOrderCounter++;

            if (_memoryBufferOrders.Count == _parquetOrdersRowGroupSize || _currentFileOrderCounter >= _maxOrdersPerFile)
            {
                await WriteParquetFiles();
                _memoryBufferOrders.Clear();
                _memoryBufferSales.Clear();
            }

            if (_currentFileOrderCounter >= _maxOrdersPerFile)
            {
                Logger.Info("New parquet file");
                _fileCounter++;
                _currentFileOrderCounter = 0;
                _appendToCurrentFile = false;
            }
        }


        public async Task WriteStaticData(IEnumerable<Customer> customers,
                                    IEnumerable<Store> stores,
                                    IEnumerable<Product> products,
                                    IEnumerable<DateExtended> dates,
                                    IEnumerable<CurrencyExchange> currencyExchanges)
        {
            var parOpt = new ParquetSerializerOptions() { Append = false };

            Log("Customers");
            var parquetCustomers = customers.OrderBy(x => x.CustomerID).Select(x => ParquetCustomer.FromCustomer(x));
            await ParquetSerializer.SerializeAsync(parquetCustomers, Path.Combine(_outputFolder, $"{Consts.CUSTOMERS}.parquet"), parOpt);

            Log("Stores");
            var parquetStores = stores.Select(x => ParquetStore.FromStore(x));
            await ParquetSerializer.SerializeAsync(parquetStores, Path.Combine(_outputFolder, $"{Consts.STORES}.parquet"), parOpt);

            Log("Products");
            var parquetProducts = products.Select(x => ParquetProduct.FromProduct(x));
            await ParquetSerializer.SerializeAsync(parquetProducts, Path.Combine(_outputFolder, $"{Consts.PRODUCTS}.parquet"), parOpt);

            Log("Dates");
            var parquetDates = dates.Select(x => ParquetDateExtended.FromDateExtended(x));
            await ParquetSerializer.SerializeAsync(parquetDates, Path.Combine(_outputFolder, $"{Consts.DATES}.parquet"), parOpt);

            Log("CurrencyExchange");
            var parquetCurrExchanges = currencyExchanges.Select(x => ParquetCurrencyExchange.FromCurrencyExchange(x));
            await ParquetSerializer.SerializeAsync(parquetCurrExchanges, Path.Combine(_outputFolder, $"{Consts.CURREXCHS}.parquet"), parOpt);
        }


        public void Close()
        {
            if (_memoryBufferOrders.Count > 0)
            {
                WriteParquetFiles().Wait();
            }

            if (_isDeltaTableOutput)
            {
                Log($"Delta Table conversion");

                if (_SOOutput.WriteOrders) WriteDeltaTableFiles(_ordersFiles, $"{Consts.ORDERS}", CreateDeltaLogBody(_ordersFiles, ParquetOrder.GETDeltaSchema()));
                if (_SOOutput.WriteOrders) WriteDeltaTableFiles(_orderRowsFiles, $"{Consts.ORDERROWS}", CreateDeltaLogBody(_orderRowsFiles, ParquetOrderRow.GETDeltaSchema()));
                if (_SOOutput.WriteSales) WriteDeltaTableFiles(_salesFiles, $"{Consts.SALES}", CreateDeltaLogBody(_salesFiles, ParquetSale.GETDeltaSchema()));

                WriteDeltaTableFiles($"{Consts.CUSTOMERS}.parquet", $"{Consts.CUSTOMERS}", CreateDeltaLogBody($"{Consts.CUSTOMERS}.parquet", ParquetCustomer.GETDeltaSchema()));
                WriteDeltaTableFiles($"{Consts.STORES}.parquet", $"{Consts.STORES}", CreateDeltaLogBody($"{Consts.STORES}.parquet", ParquetStore.GETDeltaSchema()));
                WriteDeltaTableFiles($"{Consts.PRODUCTS}.parquet", $"{Consts.PRODUCTS}", CreateDeltaLogBody($"{Consts.PRODUCTS}.parquet", ParquetProduct.GETDeltaSchema()));
                WriteDeltaTableFiles($"{Consts.DATES}.parquet", $"{Consts.DATES}", CreateDeltaLogBody($"{Consts.DATES}.parquet", ParquetDateExtended.GETDeltaSchema()));
                WriteDeltaTableFiles($"{Consts.CURREXCHS}.parquet", $"{Consts.CURREXCHS}", CreateDeltaLogBody($"{Consts.CURREXCHS}.parquet", ParquetCurrencyExchange.GETDeltaSchema()));
            }
            else
            {
                // rename parquet files
                if (_SOOutput.WriteOrders) File.Move(Path.Combine(_outputFolder, _ordersFiles[0]), Path.Combine(_outputFolder, _ordersFiles[0].Replace("_0.parquet", ".parquet")));
                if (_SOOutput.WriteOrders) File.Move(Path.Combine(_outputFolder, _orderRowsFiles[0]), Path.Combine(_outputFolder, _orderRowsFiles[0].Replace("_0.parquet", ".parquet")));
                if (_SOOutput.WriteSales) File.Move(Path.Combine(_outputFolder, _salesFiles[0]), Path.Combine(_outputFolder, _salesFiles[0].Replace("_0.parquet", ".parquet")));
            }
        }


        public string CreateDeltaLogBody(string parquetFileName, string deltaSchema)
        {
            return CreateDeltaLogBody(new List<string> { parquetFileName }, deltaSchema);
        }


        public string CreateDeltaLogBody(List<string> parquetFileList, string deltaSchema)
        {
            long unixnow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var body = new StringBuilder();

            body.AppendLine("{\"protocol\":{\"minReaderVersion\":1,\"minWriterVersion\":2}}");
            body.AppendLine("{\"metaData\":{\"id\":\"###GUID###\",\"name\":null,\"description\":null,\"format\":{\"provider\":\"parquet\",\"options\":{}},\"schemaString\":\"###SCHEMA###\",\"partitionColumns\":[],\"createdTime\":###NOW###,\"configuration\":{}}}");

            foreach (var parquetFileName in parquetFileList)
            {
                long parquetFileSize = new FileInfo(Path.Combine(_outputFolder, parquetFileName)).Length;
                string addFileLine = "{\"add\":{\"path\":\"###FILENAME###\",\"size\":###SIZE###,\"partitionValues\":{},\"modificationTime\":###NOW###,\"dataChange\":true,\"stats\":null}}";
                addFileLine = addFileLine.Replace("###FILENAME###", parquetFileName);
                addFileLine = addFileLine.Replace("###SIZE###", parquetFileSize.ToString());
                addFileLine = addFileLine.Replace("###NOW###", unixnow.ToString());
                body.AppendLine(addFileLine);
            }

            body = body.Replace("###SCHEMA###", deltaSchema.Replace("\"", "\\\""));
            body = body.Replace("###NOW###", unixnow.ToString());
            body = body.Replace("###GUID###", Guid.NewGuid().ToString());

            return body.ToString();
        }


        private void WriteDeltaTableFiles(string parquetFileName, string tableName, string deltaLogBody)
        {
            WriteDeltaTableFiles(new List<string> { parquetFileName }, tableName, deltaLogBody);
        }


        private void WriteDeltaTableFiles(List<string> parquetFileList, string tableName, string deltaLogBody)
        {
            Directory.CreateDirectory(Path.Combine(_outputFolder, tableName));
            Directory.CreateDirectory(Path.Combine(_outputFolder, tableName, "_delta_log"));

            // log file            
            File.WriteAllText(Path.Combine(_outputFolder, tableName, "_delta_log", "00000000000000000000.json"), deltaLogBody);

            // parquet file
            foreach (var parquetFileName in parquetFileList)
            {
                File.Move(Path.Combine(_outputFolder, parquetFileName), Path.Combine(_outputFolder, tableName, parquetFileName), false);
            }
        }


        private async Task WriteParquetFiles()
        {
            // Check if there is something to be written
            if (_memoryBufferOrders.Any())
            {
                _rowGroupCounter++;

                Logger.Info($"Parquet file dump > CurrentFileID: {_fileCounter} Orders: {_memoryBufferOrders.Count}  (TotalRowGroups: {_rowGroupCounter})");

                var parOpt = new ParquetSerializerOptions()
                {
                    Append = _appendToCurrentFile,
                    //CompressionMethod = ParquetLib.CompressionMethod.Gzip
                };

                if (_SOOutput.WriteOrders)
                {
                    var parquetOrders = _memoryBufferOrders.Select(order => ParquetOrder.FromOrder(order));
                    var parquetRows = _memoryBufferOrders.SelectMany(order => order.Rows.Select(row => ParquetOrderRow.FromOrderRow(order, row)));
                    string ordersFileName = $"{Consts.ORDERS}_{_fileCounter}.parquet";
                    string orderRowsFileName = $"{Consts.ORDERROWS}_{_fileCounter}.parquet";
                    await ParquetSerializer.SerializeAsync(parquetOrders, Path.Combine(_outputFolder, ordersFileName), parOpt);
                    await ParquetSerializer.SerializeAsync(parquetRows, Path.Combine(_outputFolder, orderRowsFileName), parOpt);
                    if (!_ordersFiles.Contains(ordersFileName)) _ordersFiles.Add(ordersFileName);
                    if (!_orderRowsFiles.Contains(orderRowsFileName)) _orderRowsFiles.Add(orderRowsFileName);
                }

                if (_SOOutput.WriteSales)
                {
                    var parquetSales = _memoryBufferSales.Select(sale => ParquetSale.FromSale(sale));
                    string salesFileName = $"{Consts.SALES}_{_fileCounter}.parquet";
                    await ParquetSerializer.SerializeAsync(parquetSales, Path.Combine(_outputFolder, salesFileName), parOpt);
                    if (!_salesFiles.Contains(salesFileName)) _salesFiles.Add(salesFileName);
                }

                _appendToCurrentFile = true;
            }
        }

    }

}