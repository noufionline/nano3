using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp2
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }


        public static List<Customer> GetCustomers()
        {
            return Enumerable.Range(1,100).Select(x=> new Customer{CustomerId = x,CustomerName =$"Customer : {x}"}).ToList();
        }
    }
}
