using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FirstGiving.Sdk.Api;
using System.Net.Mail;
using System.Net;

namespace FirstGiving.Sdk.Test.Integration
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenApiClient
    {
        private ApiClient apiClient;

        [SetUp]
        public void SetUp()
        {
            // Create a credentials.txt at the root of the project with your key on the first line and the secret on the second line!
            // Best way I could find to keep credentials out of the source control!
            var credentials = CredentialHelper.GetCredentials();
            this.apiClient = GetApiClient(credentials);
        }

        private static ApiClient GetApiClient(NetworkCredential credentials)
        {
            return new ApiClient(credentials.UserName, credentials.Password, new Uri("http://usapisandbox.fgdev.net/"));
        }

        private static Donation GetDonation()
        {
            return new Donation(Guid.NewGuid(), "Because I want to", 25.0M);
        }

        private static CreditCardPaymentData GetCreditCardPaymentData()
        {
            return new CreditCardPaymentData("First", "Last", "1 Main St.", "Burlington", "01803", "US", new MailAddress("test@example.org"),
                "4457010000000009", CreditCardKind.Visa, new DateTime(2012, 01, 01), "111")
            {
                State = "MA"
            };
        }

        [Test]
        public void When_saying_hello_Then_works()
        {
            string result = this.apiClient.SayHello();

            Assert.That(result, Is.EqualTo("hello"));
        }

        [Test]
        public void When_donating_by_credit_card_Then_works()
        {
            var donation = GetDonation();
            var paymentData = GetCreditCardPaymentData();

            string result = this.apiClient.DonateByCreditCard(donation, paymentData, IPAddress.Parse("127.0.0.1"));

            Assert.That(result, Is.Not.EqualTo(string.Empty));
        }

        [Test]
        public void When_using_wrong_credentials_Then_throws()
        {
            var donation = GetDonation();
            var paymentData = GetCreditCardPaymentData();

            var faultyClient = GetApiClient(new NetworkCredential("what", "what"));

            Assert.Throws<UnauthorizedException>(() => faultyClient.DonateByCreditCard(donation, paymentData, IPAddress.Parse("127.0.0.1")));
        }
    }
}
