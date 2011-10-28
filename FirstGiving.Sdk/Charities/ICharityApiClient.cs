using System;

namespace FirstGiving.Sdk.Charities
{
    public interface ICharityApiClient
    {
        Charity GetByUUID(Guid uuid);
        Charity GetByGovernmentID(string governmentID);
        Charity[] FindByName(string name);
        Uri ApiEndpoint { get; }
    }
}