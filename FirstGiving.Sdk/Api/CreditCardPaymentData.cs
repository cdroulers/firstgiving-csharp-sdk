using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    public class CreditCardPaymentData : PaymentData
    {
        public string CardNumber { get; private set; }

        public CreditCardKind CardKind { get; private set; }

        public DateTime CardExpirationDate { get; private set; }

        public string CardValidationNumber { get; private set; }

        public string BillingDescriptor { get; set; }

        public CreditCardPaymentData(string firstName, string lastName, string address1, string city, string zipCode, string country, MailAddress emailAddress,
            string cardNumber, CreditCardKind cardKind, DateTime cardExpirationDate, string cardValidationNumber)
            : base(firstName, lastName, address1, city, zipCode, country, emailAddress)
        {
            this.UpdateCardData(cardNumber, cardKind, cardExpirationDate, cardValidationNumber);
        }

        public void UpdateCardData(string cardNumber, CreditCardKind cardKind, DateTime cardExpirationDate, string cardValidationNumber)
        {
            Validate.Is.NotNullOrWhiteSpace(cardNumber, "CardNumber");
            switch (cardKind)
            {
                case CreditCardKind.AmericanExpress:
                    Validate.Is.EqualTo(cardNumber.Length, 15, "CardNumber.AmericanExpress.Length");
                    break;
                default:
                    Validate.Is.EqualTo(cardNumber.Length, 16, "CardNumber.Length");
                    break;
            }

            Validate.Is.NotNullOrWhiteSpace(cardValidationNumber, "CardValidationNumber");
            switch (cardKind)
            {
                case CreditCardKind.Visa:
                    Validate.Is.EqualTo(cardValidationNumber.Length, 3, "CardValidationNumber.Visa.CVV2.Length");
                    break;
                case CreditCardKind.MasterCard:
                    Validate.Is.EqualTo(cardValidationNumber.Length, 3, "CardValidationNumber.Visa.CVC2.Length");
                    break;
                case CreditCardKind.AmericanExpress:
                    Validate.Is.EqualTo(cardValidationNumber.Length, 4, "CardValidationNumber.Visa.CID.Length");
                    break;
            }

            this.CardNumber = cardNumber;
            this.CardKind = cardKind;
            this.CardExpirationDate = cardExpirationDate;
            this.CardValidationNumber = cardValidationNumber;
        }
    }
}
