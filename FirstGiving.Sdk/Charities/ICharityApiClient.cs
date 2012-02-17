using System;

namespace FirstGiving.Sdk.Charities
{
    /// <summary>
    ///     An interface declaring available methods for querying charities
    /// </summary>
    public interface ICharityApiClient
    {
        /// <summary>
        /// Gets a charity by FirstGiving UUID.
        /// </summary>
        /// <param name="uuid">The UUID.</param>
        /// <returns></returns>
        Charity GetByUUID(Guid uuid);
        /// <summary>
        /// Gets a charity by government ID.
        /// </summary>
        /// <param name="governmentID">The government ID.</param>
        /// <returns></returns>
        Charity GetByGovernmentID(string governmentID);
        /// <summary>
        /// Finds a charity by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Charity[] FindByName(string name);
        /// <summary>
        /// Gets the API endpoint.
        /// </summary>
        Uri ApiEndpoint { get; }
    }
}