using CsvHelper;
using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator
{

    internal class ECBExchangesReader
    {

        public static List<CurrencyExchange> GetData(string fileNameFP, bool addFutureFakeData)
        {
            // https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html
            // https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.zip 

            // Example:
            // Date,USD,JPY,BGN,CYP,CZK,DKK,EEK,GBP,HUF,LTL,LVL,MTL,PLN,ROL,RON,SEK,SIT,SKK,CHF,ISK,NOK,HRK,RUB,TRL,TRY,AUD,BRL,CAD,CNY,HKD,IDR,ILS,INR,KRW,MXN,MYR,NZD,PHP,SGD,THB,ZAR,
            // 2024-04-26,1.0714,168.03,1.9558,N/A,25.164,7.4573,N/A,0.85643,392.28,N/A,N/A,N/A,4.3205,N/A,4.9764,11.7052,N/A,N/A,0.9779,150.3,11.7995,N/A,N/A,N/A,34.8036,1.6392,5.5208,1.4632,7.7638,8.3873,17385.77,4.0798,89.3191,1474.65,18.4672,5.1079,1.801,61.83,1.4587,39.583,20.2037,
            // 2024-04-25,1.072,166.76,1.9558,N/A,25.152,7.4587,N/A,0.85675,392.98,N/A,N/A,N/A,4.3165,N/A,4.9761,11.639,N/A,N/A,0.9792,150.1,11.714,N/A,N/A,N/A,34.8475,1.6415,5.505,1.4659,7.7682,8.392,17355.14,4.0646,89.3155,1473.23,18.267,5.1215,1.799,61.94,1.4581,39.691,20.3377,

            // Important: the source file:
            //   - contains exchange rate from Euro to other currencies
            //   - is ordered by descending date
            //   - only contains data for effective days of currencies exchange. So, there are holes to be filled.

            var outList = new List<CurrencyExchange>();

            string[] currencyCodes = { "AUD", "CAD", "EUR", "GBP", "USD" };

            var csv = new CsvReader(new StreamReader(fileNameFP), CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();


            DateTime previousDT = DateTime.MinValue;

            while (csv.Read())
            {
                var dt = DateTime.ParseExact(csv.GetField<String>("Date"), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (previousDT == DateTime.MinValue) { previousDT = dt.AddDays(1); }

                for (int i = 0; i < previousDT.Subtract(dt).TotalDays; i++)
                {
                    var exchangeDT = previousDT.AddDays(-i - 1);
                    //Console.WriteLine($"{dt} {exchangeDT}");

                    foreach (var currencyCodeFrom in currencyCodes)
                    {
                        foreach (var currencyCodeTo in currencyCodes)
                        {
                            decimal fromExchange = (currencyCodeFrom == "EUR") ? 1.0M : (decimal)csv.GetField<double>(currencyCodeFrom);
                            decimal toExchange = (currencyCodeTo == "EUR") ? 1.0M : (decimal)csv.GetField<double>(currencyCodeTo);
                            decimal exchangeRate = decimal.Round(1 / fromExchange * toExchange, 5);                            
                            outList.Add(new CurrencyExchange() { Date = exchangeDT, FromCurrency = currencyCodeFrom, ToCurrency = currencyCodeTo, Exchange = exchangeRate });
                            //Console.WriteLine($"{exchangeDT} {currencyCodeFrom} {currencyCodeTo} {exchangeRate.ToString("0.00000")}");
                        }
                    }
                }

                previousDT = dt;
            }

            if (addFutureFakeData)
            {
                AddFutureData(outList);
            }

            return outList;
        }


        private static void AddFutureData(List<CurrencyExchange> dataSet)
        {
            var refDT = dataSet.Max(x => x.Date);

            var fakeData = new List<CurrencyExchange>();

            foreach (var item in dataSet)
            {
                double deltaDays = refDT.Subtract(item.Date).TotalDays;
                var fakeItem = new CurrencyExchange() { Date = refDT.AddDays(deltaDays + 1), FromCurrency = item.FromCurrency, ToCurrency = item.ToCurrency, Exchange = item.Exchange };
                fakeData.Add(fakeItem);
            }

            var temp = new List<CurrencyExchange>();
            temp.AddRange(dataSet);
            temp.AddRange(fakeData);

            dataSet.Clear();
            dataSet.AddRange(temp.OrderBy(x => x.Date).ThenBy(x => x.FromCurrency).ThenBy(x => x.ToCurrency));
        }

    }

}
