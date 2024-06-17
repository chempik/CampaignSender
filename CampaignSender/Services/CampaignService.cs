using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CampaignSender;
using CampaignSender.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

public class CampaignService
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITemplateRepository _templateRepository;
    private readonly IEmailService _emailService;

    public CampaignService(ICampaignRepository campaignRepository,
                           ICustomerRepository customerRepository,
                           ITemplateRepository templateRepository,
                           IEmailService emailService)
    {
        _campaignRepository = campaignRepository;
        _customerRepository = customerRepository;
        _templateRepository = templateRepository;
        _emailService = emailService;
    }

    public async Task SendCampaignAsync(Campaign campaign)
    {
        var customers = await _customerRepository.GetCustomersAsync();
        var template = await _templateRepository.GetTemplateAsync(campaign.TemplateId);

        var tasks = new List<Task>();

        foreach (var customer in customers)
        {
            if (campaign.Condition.Evaluate(customer))
            {
                tasks.Add(SendEmailToCustomerAsync(template, customer));
            }
        }

        await Task.WhenAll(tasks);
    }

    private async Task SendEmailToCustomerAsync(Template template, Customer customer)
    {
        var message = await ReplacePlaceholdersAsync(template.Content, customer);
        var email = GenerateEmailForCustomer(customer);
        await _emailService.SendEmailAsync(email, "Campaign", message);
    }

    public async Task<string> ReplacePlaceholdersAsync(string templateContent, Customer customer)
    {
        var replacements = new Dictionary<string, string>
    {
        { "{{Customer.Name}}", customer.Name },
        { "{{Customer.Deposit}}", customer.Deposit.ToString() }
    };

        foreach (var placeholder in replacements)
        {
            templateContent = templateContent.Replace(placeholder.Key, placeholder.Value);
        }

        return templateContent;
    }

    private string GenerateEmailForCustomer(Customer customer)
    {
        return $"{customer.Name.Replace(" ", ".").ToLower()}@example.com";
    }
}
