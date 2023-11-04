namespace Utilities.Common.TokenHelpers
{
    /// <summary>
    /// The settings to send an email.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Gets or sets the host to send from.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port to use in sending.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the username to send with.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the pw to auth with.
        /// </summary>
        public string Code { get; set; }
    }
}
