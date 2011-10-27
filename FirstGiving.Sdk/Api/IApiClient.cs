using System;
using System.Net;

namespace FirstGiving.Sdk.Api
{
    public interface IApiClient
    {
        string ApplicationKey { get; }
        string SecurityToken { get; }
        Uri ApiEndpoint { get; }
        string DonateByCreditCard(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress);
        string DonateByCreditCardRecurring(Donation donation, CreditCardPaymentData paymentData, IPAddress remoteAddress, BillingFrequency frequency, int? term);
    }
}