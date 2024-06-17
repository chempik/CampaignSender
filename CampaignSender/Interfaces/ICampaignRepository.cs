using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampaignSender.Interfaces
{
    public interface ICampaignRepository
    {
        Task<List<Campaign>> GetScheduledCampaignsAsync();
    }
}
