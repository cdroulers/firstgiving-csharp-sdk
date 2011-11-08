using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FirstGiving.Sdk.Api
{
    public class TestApiClient : IApiClient
    {
        private readonly IApiClient decorated;

        public TestApiClient(IApiClient decorated)
        {
            this.decorated = decorated;
        }

        public string ApplicationKey
        {
            get { return this.decorated.ApplicationKey; }
        }

        public string SecurityToken
        {
            get { return this.decorated.SecurityToken; }
        }

        public Uri ApiEndpoint
        {
            get { return this.decorated.ApiEndpoint; }
        }

        private void EditData(CreditCardPaymentData paymentData)
        {
            paymentData.Address1 = "1 Main St.";
            paymentData.City = "Burlington";
            paymentData.ZipCode = "01803";
            paymentData.Country = "US";
            paymentData.UpdateCardData("4457010000000009", CreditCardKind.Visa, new DateTime(2012, 01, 01), "111");
        }

        public string DonateByCreditCard(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress)
        {
            EditData(paymentData);
            return this.decorated.DonateByCreditCard(donation, paymentData, remoteAddress);
        }

        public string DonateByCreditCardRecurring(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress, BillingFrequency frequency, int? term)
        {
            EditData(paymentData);
            return this.decorated.DonateByCreditCardRecurring(donation, paymentData, remoteAddress, frequency, term);
        }
    }
}
