using CampaignSender.Interfaces;
using CampaignSender.Repositories;
using CampaignSender.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CampaignSender
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = BuildServiceProvider();

            var timerService = serviceProvider.GetService<TimerService>();
            timerService.Start();

            Console.WriteLine("Campaign sender started. Press any key to exit.");
            Console.ReadKey();

            timerService.Stop();
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddSingleton<CampaignService>();
            services.AddSingleton<TimerService>();

            // Register the repository implementations
            services.AddSingleton<ICampaignRepository, CampaignRepository>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<ITemplateRepository, TemplateRepository>();
            services.AddScoped<IEmailService, EmailService>();

            return services.BuildServiceProvider();
        }
    }
}