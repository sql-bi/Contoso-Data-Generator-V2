using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGenerator
{

    public static class Consts
    {
        #pragma warning disable format

        public const string DOWNLOAD_BASE_URL = "https://github.com/sql-bi/Contoso-Data-Generator-V2-Data/releases/download/static-files/";

        public const string ORDERS    = "orders";
        public const string ORDERROWS = "orderrows";
        public const string SALES     = "sales";
        public const string CURREXCHS = "currencyexchange";
        public const string CUSTOMERS = "customer";
        public const string DATES     = "date";
        public const string PRODUCTS  = "product";
        public const string STORES    = "store";

        public const string FILE_ECB_EXCH_CSV = "ECB_eurofxref-hist.csv";

        #pragma warning restore format
    }

}
