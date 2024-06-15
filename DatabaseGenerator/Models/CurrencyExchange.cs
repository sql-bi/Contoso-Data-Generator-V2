using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator.Models
{
    public class CurrencyExchange
    {
        public DateTime Date { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Exchange { get; set; }

        public override string ToString()
        {
            return Date + " : " + FromCurrency + " : " + ToCurrency + " : " + Exchange;
        }
    }
}
