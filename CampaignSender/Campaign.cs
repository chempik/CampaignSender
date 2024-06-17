using System;

namespace CampaignSender
{
    public class Campaign
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public Condition Condition { get; set; }
        public DateTime SendTime { get; set; }
        public int Priority { get; set; }
    }
}
