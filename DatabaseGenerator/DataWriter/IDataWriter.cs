using DatabaseGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseGenerator.DataWriter
{

    public interface IDataWriter
    {
        void Init();

        Task WriteOrderWithRows(Order order,
                                IEnumerable<Sale> sales);

        Task WriteStaticData(IEnumerable<Customer> customers,
                             IEnumerable<Store> stores,
                             IEnumerable<Product> products,
                             IEnumerable<DateExtended> dates,
                             IEnumerable<CurrencyExchange> currencyExchanges);

        void Close();

    }

}
