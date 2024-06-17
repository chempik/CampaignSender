using CampaignSender.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampaignSender.Services
{
    internal class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            await Task.Delay(100);

            Console.WriteLine($"Sending email to: {toEmail}, Subject: {subject}, Body: {body}");
        }
    }
}
