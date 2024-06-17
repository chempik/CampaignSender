using CampaignSender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignSender.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<List<Customer>> GetCustomersAsync()
        {
            return Task.FromResult(new List<Customer>
        {
            new Customer { Id = 1, Age = 53, Gender = "Male", City = "London", Deposit = 104, NewCustomer = 0, Name = "John Doe" },
        });
        }
    }
}
