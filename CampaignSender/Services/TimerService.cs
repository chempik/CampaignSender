using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Timers;
using System.Collections.Generic;
using System;
using System.Linq;
using Timer = System.Timers.Timer;

namespace CampaignSender.Services
{
    public class TimerService
    {
        private readonly CampaignService _campaignService;
        private readonly Timer _timer;

        public TimerService(CampaignService campaignService)
        {
            _campaignService = campaignService;
            _timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            _timer.Elapsed += TimerElapsedAsync;
            _timer.AutoReset = true;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
        }

        private async void TimerElapsedAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                var campaigns = GetScheduledCampaigns().ToList();
                foreach (var campaign in campaigns)
                {
                    await _campaignService.SendCampaignAsync(campaign);
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
            }
        }

        private IEnumerable<Campaign> GetScheduledCampaigns()
        {
            return new List<Campaign>
        {
            new Campaign
            {
                Id = 1,
                TemplateId = 1,
                Condition = new MaleCondition(),
                SendTime = DateTime.Now.AddMinutes(5),
                Priority = 1
            },
            new Campaign
            {
                Id = 2,
                TemplateId = 2,
                Condition = new AgeCondition(45),
                SendTime = DateTime.Now.AddMinutes(10),
                Priority = 2
            },
        };
        }
    }
}
