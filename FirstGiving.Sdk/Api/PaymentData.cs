using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace FirstGiving.Sdk.Api
{
    public abstract class PaymentData
    {
        private string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 10, "Title.Length");
                this.title = value;
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "FirstName");
                Validate.Is.LowerThanOrEqualTo(value.Length, 100, "FirstName.Length");
                this.firstName = value;
            }
        }

        private string middleName;
        public string MiddleName
        {
            get { return this.middleName; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 100, "MiddleName.Length");
                this.middleName = value;
            }
        }

        private string lastName;
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "LastName");
                Validate.Is.LowerThanOrEqualTo(value.Length, 100, "LastName.Length");
                this.lastName = value;
            }
        }

        private string address1;
        public string Address1
        {
            get { return this.address1; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "Address1");
                Validate.Is.LowerThanOrEqualTo(value.Length, 255, "Address1.Length");
                this.address1 = value;
            }
        }

        private string address2;
        public string Address2
        {
            get { return this.address2; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 255, "Address2.Length");
                this.address2 = value;
            }
        }

        private string address3;
        public string Address3
        {
            get { return this.address3; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 255, "Address3.Length");
                this.address3 = value;
            }
        }

        private string city;
        public string City
        {
            get { return this.city; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "City");
                Validate.Is.LowerThanOrEqualTo(value.Length, 35, "City.Length");
                this.city = value;
            }
        }

        private string state;
        public string State
        {
            get { return this.state; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "State");
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 30, "State.Length");
                this.state = value;
            }
        }

        private string zipCode;
        public string ZipCode
        {
            get { return this.zipCode; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "ZipCode");
                Validate.Is.LowerThanOrEqualTo(value.Length, 20, "ZipCode.Length");
                this.zipCode = value;
            }
        }

        private string country;
        public string Country
        {
            get { return this.country; }
            set
            {
                Validate.Is.NotNullOrWhiteSpace(value, "Country");
                Validate.Is.EqualTo(value.Length, 2, "Country.Length");
                this.country = value;
            }
        }

        private MailAddress emailAddress;
        public MailAddress EmailAddress
        {
            get { return this.emailAddress; }
            set
            {
                Validate.Is.NotNull(value, "EmailAddress");
                Validate.Is.LowerThanOrEqualTo(value.Address.Length, 100, "EmailAddress.Length");
                this.emailAddress = value;
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return this.phoneNumber; }
            set
            {
                Validate.Is.LowerThanOrEqualTo((value ?? string.Empty).Length, 20, "PhoneNumber.Length");
                this.phoneNumber = value;
            }
        }

        protected PaymentData(string firstName, string lastName, string address1, string city, string zipCode, string country, MailAddress emailAddress)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address1 = address1;
            this.City = city;
            this.ZipCode = zipCode;
            this.Country = country;
            this.EmailAddress = emailAddress;
        }
    }
}
