using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator.DataWriter.Csv
{
    public class CsvCurrencyExchange
    {
        public DateTime Date { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Exchange { get; set; }


        public static CsvCurrencyExchange FromCurrencyExchange(CurrencyExchange ce)
        {
            return new CsvCurrencyExchange()
            {
                Date = ce.Date,
                FromCurrency = ce.FromCurrency,
                ToCurrency = ce.ToCurrency,
                Exchange = ce.Exchange,
            };
        }
    }
}
