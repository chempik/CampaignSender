using System.Threading.Tasks;

namespace CampaignSender.Interfaces
{
    public interface ITemplateRepository
    {
        Task<Template> GetTemplateAsync(int templateId);
    }
}