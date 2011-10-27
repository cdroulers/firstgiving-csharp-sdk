using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace FirstGiving.Sdk
{
    public class CreditCardPaymentData : PaymentData
    {
        private string cardNumber;
        public string CardNumber
        {
            get { return this.cardNumber; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "CardNumber");
                Validate.Is.EqualTo(value.Length, 16, "CardNumber.Length");
                this.cardNumber = value;
            }
        }

        public CreditCardKind CardKind { get; set; }

        public DateTime CardExpirationDate { get; set; }

        private string cardValidationNumber;
        public string CardValidationNumber
        {
            get { return this.cardValidationNumber; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "CardValidationNumber");
                switch (this.CardKind)
                {
                    case CreditCardKind.Visa:
                        Validate.Is.EqualTo(value.Length, 3, "CardValidationNumber.Visa.CVV2.Length");
                        break;
                    case CreditCardKind.MasterCard:
                        Validate.Is.EqualTo(value.Length, 3, "CardValidationNumber.Visa.CVC2.Length");
                        break;
                    case CreditCardKind.AmericanExpress:
                        Validate.Is.EqualTo(value.Length, 4, "CardValidationNumber.Visa.CID.Length");
                        break;
                }
                this.cardValidationNumber = value;
            }
        }

        public string BillingDescriptor { get; set; }

        public CreditCardPaymentData(string firstName, string lastName, string address1, string city, string zipCode, string country, MailAddress emailAddress,
            string cardNumber, CreditCardKind cardKind, DateTime cardExpirationDate, string cardValidationNumber)
            : base(firstName, lastName, address1, city, zipCode, country, emailAddress)
        {
            this.CardNumber = cardNumber;
            this.CardKind = cardKind;
            this.CardExpirationDate = cardExpirationDate;
            this.CardValidationNumber = cardValidationNumber;
        }
    }
}
