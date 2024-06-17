using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignSender.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomersAsync();
    }
}
