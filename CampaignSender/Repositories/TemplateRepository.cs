using CampaignSender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignSender.Repositories
{
    public class TemplateRepository : ITemplateRepository
    {
        public Task<Template> GetTemplateAsync(int templateId)
        {
            return Task.FromResult(new Template { Id = templateId, Content = "Sample Template Content" });
        }
    }
}
