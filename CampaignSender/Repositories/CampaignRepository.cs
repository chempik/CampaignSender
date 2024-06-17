using CampaignSender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignSender.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        public Task<List<Campaign>> GetScheduledCampaignsAsync()
        {

            return Task.FromResult(new List<Campaign>
        {
            new Campaign { Id = 1, TemplateId = 1, Condition = new MaleCondition(), SendTime = DateTime.Now.AddMinutes(5), Priority = 1 },
            new Campaign { Id = 2, TemplateId = 2, Condition = new AgeCondition(45), SendTime = DateTime.Now.AddMinutes(10), Priority = 2 }
        });
        }
    }
}
