using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;


namespace DatabaseGenerator
{

    public class Program
    {

        static async Task Main(string[] args)
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            Console.WriteLine($"{assemblyName.Name} - {assemblyName.Version}");
            Console.WriteLine();
            Console.WriteLine($"https://github.com/sql-bi/Contoso-Data-Generator-V2");
            Console.WriteLine();


            if (args.Length == 0)
            {
                Console.WriteLine("USAGE:  databasegenerator.exe  configfile  datafile  outputfolder  cachefolder  [param:OrdersCount=nnnnnnn] ");
                return;
            }

            string configFile = args[0];
            string datafile = args[1];
            string outputFolder = args[2];
            string cacheFolder = args[3];

            // read config
            var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(Path.Combine(configFile)), ConfigSerializerContext.Default.Config);
            InjectConfigValuesFromCommandLine(config, args);
            config.SalesOrdersOut = new SOOutput(config.SalesOrders);

            // run engine
            await new Engine(datafile, outputFolder, cacheFolder, config).Exec();
        }


        private static void InjectConfigValuesFromCommandLine(Config cfg, string[] args)
        {
            Func<string[], string, string> getParamValue = (args, paramName) => { return args.Where(x => x.StartsWith($"param:{paramName}=")).Select(x => x.Split('=')[1]).FirstOrDefault(); };
            Func<string, int, int> convToInt = (s, defValue) => { return string.IsNullOrEmpty(s) ? defValue : int.Parse(s); };
            Func<string, double, double> convToDouble = (s, defValue) => { return string.IsNullOrEmpty(s) ? defValue : Double.Parse(s, CultureInfo.InvariantCulture); };
            Func<string, DateTime?, DateTime?> convToDT = (s, defValue) => { return string.IsNullOrEmpty(s) ? defValue : DateTime.ParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture); };
            Func<string, string, string> convToString = (s, defValue) => { return string.IsNullOrEmpty(s) ? defValue : s; };

            cfg.OrdersCount = convToInt(getParamValue(args, "OrdersCount"), cfg.OrdersCount);
            cfg.CutDateBefore = convToDT(getParamValue(args, "CutDateBefore"), cfg.CutDateBefore);
            cfg.CutDateAfter = convToDT(getParamValue(args, "CutDateAfter"), cfg.CutDateAfter);
            cfg.CustomerPercentage = convToDouble(getParamValue(args, "CustomerPercentage"), cfg.CustomerPercentage);
            cfg.OutputFormat = convToString(getParamValue(args, "OutputFormat"), cfg.OutputFormat);
            cfg.DeltaTableOrdersPerFile = convToInt(getParamValue(args, "DeltaTableOrdersPerFile"), cfg.DeltaTableOrdersPerFile);
            cfg.ParquetOrdersRowGroupSize = convToInt(getParamValue(args, "ParquetOrdersRowGroupSize"), cfg.ParquetOrdersRowGroupSize);
            cfg.StartDT = convToDT(getParamValue(args, "StartDT"), cfg.StartDT).Value;
            cfg.YearsCount = convToInt(getParamValue(args, "YearsCount"), cfg.YearsCount);
        }

    }

}
