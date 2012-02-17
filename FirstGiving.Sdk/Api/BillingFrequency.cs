using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    /// <summary>
    /// Credit card recurring billing frequency
    /// </summary>
    public enum BillingFrequency
    {
        /// <summary>
        /// Monthly
        /// </summary>
        Monthly,
        /// <summary>
        /// Quarterly
        /// </summary>
        Quarterly,
        /// <summary>
        /// Semi-yearly
        /// </summary>
        SemiYearly,
        /// <summary>
        /// Yearly
        /// </summary>
        Yearly,
    }
}
