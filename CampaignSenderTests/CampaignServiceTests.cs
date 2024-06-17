using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampaignSender;
using CampaignSender.Interfaces;

namespace CampaignSenderTests
{
    [TestFixture]
    public class CampaignServiceTests
    {
        private Mock<ICampaignRepository> _campaignRepositoryMock;
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<ITemplateRepository> _templateRepositoryMock;
        private Mock<IEmailService> _emailServiceMock;
        private CampaignService _campaignService;

        [SetUp]
        public void Setup()
        {
            _campaignRepositoryMock = new Mock<ICampaignRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _templateRepositoryMock = new Mock<ITemplateRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            _campaignService = new CampaignService(
                _campaignRepositoryMock.Object,
                _customerRepositoryMock.Object,
                _templateRepositoryMock.Object,
                _emailServiceMock.Object);
        }

        [Test]
        public async Task SendCampaignAsync_ShouldSendCampaignToAllQualifiedCustomers()
        {
            // Arrange
            var campaign = new Campaign
            {
                Id = 1,
                TemplateId = 1,
                Condition = new MaleCondition(),
                SendTime = DateTime.Now,
                Priority = 1
            };

            var customers = new List<Customer>
            {
                new Customer { Id = 1, Age = 35, Gender = "Male", City = "New York", Deposit = 200, NewCustomer = 0, Name = "John Doe" },
                new Customer { Id = 2, Age = 40, Gender = "Male", City = "Los Angeles", Deposit = 500, NewCustomer = 0, Name = "Mike Smith" }
            };

            var template = new Template { Id = 1, Content = "Hello {{Customer.Name}}" };

            _customerRepositoryMock.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);
            _templateRepositoryMock.Setup(repo => repo.GetTemplateAsync(It.IsAny<int>())).ReturnsAsync(template);
            _emailServiceMock.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _campaignService.SendCampaignAsync(campaign);

            // Assert
            _customerRepositoryMock.Verify(repo => repo.GetCustomersAsync(), Times.Once);
            _templateRepositoryMock.Verify(repo => repo.GetTemplateAsync(It.IsAny<int>()), Times.Once);

            foreach (var customer in customers)
            {
                if (campaign.Condition.Evaluate(customer))
                {
                    var expectedMessage = $"Hello {customer.Name}";
                    var expectedEmail = $"{customer.Name.Replace(" ", ".").ToLower()}@example.com";
                    _emailServiceMock.Verify(service => service.SendEmailAsync(expectedEmail, "Campaign", expectedMessage), Times.Once);
                }
            }
        }

        [Test]
        public async Task ReplacePlaceholdersAsync_ShouldReplaceCustomerName()
        {
            // Arrange
            var templateContent = "Hello {{Customer.Name}}";
            var customer = new Customer { Name = "John Doe" };

            // Act
            var result = await _campaignService.ReplacePlaceholdersAsync(templateContent, customer);

            // Assert
            Assert.AreEqual("Hello John Doe", result);
        }

        [Test]
        public async Task SendCampaignAsync_ShouldNotSendEmailsIfNoCustomersFound()
        {
            // Arrange
            var campaign = new Campaign
            {
                Id = 1,
                TemplateId = 1,
                Condition = new MaleCondition(),
                SendTime = DateTime.Now,
                Priority = 1
            };

            var customers = new List<Customer>();

            var template = new Template { Id = 1, Content = "Hello {{Customer.Name}}" };

            _customerRepositoryMock.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);
            _templateRepositoryMock.Setup(repo => repo.GetTemplateAsync(It.IsAny<int>())).ReturnsAsync(template);

            // Act
            await _campaignService.SendCampaignAsync(campaign);

            // Assert
            _emailServiceMock.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
