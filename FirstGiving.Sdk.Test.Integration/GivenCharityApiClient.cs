using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net.Mail;
using System.Net;
using FirstGiving.Sdk.Charities;

namespace FirstGiving.Sdk.Test.Integration
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GivenCharityApiClient
    {
        private CharityApiClient apiClient;

        [SetUp]
        public void SetUp()
        {
            this.apiClient = GetApiClient();
        }

        private static CharityApiClient GetApiClient()
        {
            return new CharityApiClient(new Uri("http://graphapi.firstgiving.com/"));
        }

        [Test]
        public void When_getting_charity_by_uuid_Then_returns_right_data()
        {
            var uuid = new Guid("b62c2436-edd0-11df-ab8c-4061860da51d");

            var actual = this.apiClient.GetByUUID(uuid);

            Assert.That(actual.UUID, Is.EqualTo(uuid));
            Assert.That(actual.Address, Is.EqualTo("190 N 10TH ST STE 303 BROOKLYN NY 11211-9318"));
            Assert.That(actual.Address1, Is.EqualTo("190 N 10TH ST STE 303"));
            Assert.That(actual.Address2, Is.EqualTo(string.Empty));
            Assert.That(actual.Address3, Is.EqualTo(string.Empty));
            Assert.That(actual.AddressLines, Is.EqualTo(string.Empty));
            Assert.That(actual.Alias, Is.EqualTo(string.Empty));
            Assert.That(actual.AreaCode, Is.EqualTo(string.Empty));
            Assert.That(actual.CategoryCode, Is.EqualTo("V20"));
            Assert.That(actual.City, Is.EqualTo("BROOKLYN"));
            Assert.That(actual.Country, Is.EqualTo("US"));
            Assert.That(actual.County, Is.EqualTo("NY"));
            Assert.That(actual.GovernmentID, Is.EqualTo("260046127"));
            Assert.That(actual.Latitude, Is.EqualTo(40.6594));
            Assert.That(actual.Longitude, Is.EqualTo(-73.9625));
            Assert.That(actual.Name, Is.EqualTo("1 1 1 ONE"));
            Assert.That(actual.PhoneNumber, Is.EqualTo(string.Empty));
            Assert.That(actual.PostalCode, Is.EqualTo("11211-9318"));
            Assert.That(actual.Region, Is.EqualTo("NY"));
            Assert.That(actual.Revoked, Is.EqualTo(false));
            Assert.That(actual.Url, Is.EqualTo(null));
        }

        [Test]
        public void When_getting_inexistant_charity_Then_throws()
        {
            Assert.Throws<NotFoundException>(() => this.apiClient.GetByUUID(Guid.Empty));
        }

        [Test]
        public void When_getting_charity_by_government_id_Then_returns_right_data()
        {
            var actual = this.apiClient.GetByGovernmentID("260046127");

            Assert.That(actual.UUID, Is.EqualTo(new Guid("b62c2436-edd0-11df-ab8c-4061860da51d")));
            Assert.That(actual.Address, Is.EqualTo("190 N 10TH ST STE 303 BROOKLYN NY 11211-9318"));
            Assert.That(actual.Address1, Is.EqualTo("190 N 10TH ST STE 303"));
            Assert.That(actual.Address2, Is.EqualTo(string.Empty));
            Assert.That(actual.Address3, Is.EqualTo(string.Empty));
            Assert.That(actual.AddressLines, Is.EqualTo(string.Empty));
            Assert.That(actual.Alias, Is.EqualTo(string.Empty));
            Assert.That(actual.AreaCode, Is.EqualTo(string.Empty));
            Assert.That(actual.CategoryCode, Is.EqualTo("V20"));
            Assert.That(actual.City, Is.EqualTo("BROOKLYN"));
            Assert.That(actual.Country, Is.EqualTo("US"));
            Assert.That(actual.County, Is.EqualTo("NY"));
            Assert.That(actual.GovernmentID, Is.EqualTo("260046127"));
            Assert.That(actual.Latitude, Is.EqualTo(40.6594));
            Assert.That(actual.Longitude, Is.EqualTo(-73.9625));
            Assert.That(actual.Name, Is.EqualTo("1 1 1 ONE"));
            Assert.That(actual.PhoneNumber, Is.EqualTo(string.Empty));
            Assert.That(actual.PostalCode, Is.EqualTo("11211-9318"));
            Assert.That(actual.Region, Is.EqualTo("NY"));
            Assert.That(actual.Revoked, Is.EqualTo(false));
            Assert.That(actual.Url, Is.EqualTo(null));
        }

        [Test]
        public void When_getting_charity_by_name_Then_returns_right_data()
        {
            var results = this.apiClient.FindByName("1 1 1 ONE");

            Assert.That(results, Has.Length.EqualTo(1));
            var actual = results.First();
            Assert.That(actual.UUID, Is.EqualTo(new Guid("b62c2436-edd0-11df-ab8c-4061860da51d")));
            Assert.That(actual.Address, Is.EqualTo("190 N 10TH ST STE 303 BROOKLYN NY 11211-9318"));
            Assert.That(actual.Address1, Is.EqualTo("190 N 10TH ST STE 303"));
            Assert.That(actual.Address2, Is.EqualTo(string.Empty));
            Assert.That(actual.Address3, Is.EqualTo(string.Empty));
            Assert.That(actual.AddressLines, Is.EqualTo(string.Empty));
            Assert.That(actual.Alias, Is.EqualTo(string.Empty));
            Assert.That(actual.AreaCode, Is.EqualTo(string.Empty));
            Assert.That(actual.CategoryCode, Is.EqualTo("V20"));
            Assert.That(actual.City, Is.EqualTo("BROOKLYN"));
            Assert.That(actual.Country, Is.EqualTo("US"));
            Assert.That(actual.County, Is.EqualTo("NY"));
            Assert.That(actual.GovernmentID, Is.EqualTo("260046127"));
            Assert.That(actual.Latitude, Is.EqualTo(40.6594));
            Assert.That(actual.Longitude, Is.EqualTo(-73.9625));
            Assert.That(actual.Name, Is.EqualTo("1 1 1 ONE"));
            Assert.That(actual.PhoneNumber, Is.EqualTo(string.Empty));
            Assert.That(actual.PostalCode, Is.EqualTo("11211-9318"));
            Assert.That(actual.Region, Is.EqualTo("NY"));
            Assert.That(actual.Revoked, Is.EqualTo(false));
            Assert.That(actual.Url, Is.EqualTo(null));
        }
    }
}
