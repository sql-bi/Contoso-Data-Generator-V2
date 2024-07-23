using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace DatabaseGenerator
{

    // -----------------
    // Required for serialization because JsonSerializerIsReflectionEnabledByDefault = false
    [JsonSerializable(typeof(Config))]
    internal partial class ConfigSerializerContext : JsonSerializerContext
    {        
    }
    // -----------------


    public class Config
    {

        // Class used for reading configuration file by json deserialization. Modify carefully.

        public int OrdersCount { get; set; }
        public DateTime StartDT { get; set; }
        public int YearsCount { get; set; }
        public DateTime? CutDateBefore { get; set; }
        public DateTime? CutDateAfter { get; set; }
        public double CustomerPercentage { get; set; }
        public int CustomerFakeGenerator { get; set; }   // >0 : use fake customers

        public string OutputFormat { get; set; }   // CSV PARQUET DELTATABLE
        public string SalesOrders { get; set; }    // SALES ORDERS BOTH

        public int DeltaTableOrdersPerFile { get; set; }
        public int ParquetOrdersRowGroupSize { get; set; }

        public int? CsvMaxOrdersPerFile { get; set; }
        public int? CsvGzCompression { get; set; }


        public DaysWeight DaysWeight { get; set; }

        public double[] OrderRowsWeights { get; set; }
        public double[] OrderQuantityWeights { get; set; }
        public double[] DiscountWeights { get; set; }
        public double[] OnlinePerCent { get; set; }
        public double[] DeliveryDateLambdaWeights { get; set; }
        public Dictionary<string, string> CountryCurrency { get; set; }


        public List<AnnualSpike> AnnualSpikes { get; set; }

        public List<OneTimeSpike> OneTimeSpikes { get; set; }

        public CustomerActivity CustomerActivity { get; set; }


        // --- Filled post deserialization ---
        public SOOutput SalesOrdersOut { get; set; }

    }


    public class DaysWeight
    {
        public bool DaysWeightConstant { get; set; }
        public double[] DaysWeightPoints { get; set; }
        public double[] DaysWeightValues { get; set; }
        public bool DaysWeightAddSpikes { get; set; }
        public double[] WeekDaysFactor { get; set; }
        public double DayRandomness { get; set; }
    }


    public class OneTimeSpike
    {
        public DateTime DT1 { get; set; }
        public DateTime DT2 { get; set; }
        public double Factor { get; set; }
    }


    public class AnnualSpike
    {
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public double Factor { get; set; }
    }


    public class CustomerActivity
    {
        public double[] StartDateWeightPoints { get; set; }
        public double[] StartDateWeightValues { get; set; }
        public double[] EndDateWeightPoints { get; set; }
        public double[] EndDateWeightValues { get; set; }
    }


    public class SOOutput
    {
        private bool _orders = false;
        private bool _sales = false;

        public SOOutput(string outputType)
        {
            outputType = outputType.ToUpper();
            _orders = outputType == "ORDERS" || outputType == "BOTH";
            _sales = outputType == "SALES" || outputType == "BOTH";
            if (!_orders && !_sales) throw new Exception($"Unknown option for [SalesOrders] - '{outputType}'");
        }

        public bool WriteOrders { get { return _orders; } }
        public bool WriteSales { get { return _sales; } }
    }

}
