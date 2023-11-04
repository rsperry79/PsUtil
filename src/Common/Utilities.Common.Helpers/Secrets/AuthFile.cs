#pragma warning disable IDE1006 // Naming Styles

namespace Utilities.Common.Helpers.Secrets
{
    /// <summary>
    /// Defines the <see cref="AuthFile" />.
    /// </summary>
    public class AuthFile
    {
        /// <summary>
        /// Gets or sets the clientId.
        /// </summary>
        public string clientId { get; set; }

        /// <summary>
        /// Gets or sets the clientSecret.
        /// </summary>
        public string clientSecret { get; set; }

        /// <summary>
        /// Gets or sets the tenantId.
        /// </summary>
        public string tenantId { get; set; }

        /// <summary>
        /// Gets or sets the subscriptionId.
        /// </summary>
        public string subscriptionId { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles
