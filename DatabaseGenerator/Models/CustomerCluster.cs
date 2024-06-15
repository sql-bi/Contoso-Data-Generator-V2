using DatabaseGenerator.Fast;
using System;


namespace DatabaseGenerator.Models
{
    public class CustomerCluster
    {
        public int ClusterID { get; set; }
        public double OrdersWeight { get; set; }
        public double CustomersWeight { get; set; }
        

        public CustomerListFast CustomersFast { get; set; }        
    }
}
