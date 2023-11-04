using System;
using System.Threading.Tasks;

using Microsoft.Identity.Client;

namespace Utilities.Common.TokenHelpers
{
    /// <summary>
    /// Defines the <see cref="TokenGet" />
    /// </summary>
    public class TokenGet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenGet"/> class.
        /// </summary>
        public TokenGet()
        {
            AppConfiguration config = AppConfiguration.ReadFromJsonFile("appsettings.json");
            PublicClientApplicationOptions appConfig = config.PublicClientApplicationOptions;
            IPublicClientApplication app = PublicClientApplicationBuilder.CreateWithApplicationOptions(appConfig).Build();

            this.tokenAcquisitionHelper = new PublicAppUsingDeviceCodeFlow(app);
        }

        /// <summary>
        /// Defines the tokenAcquisitionHelper
        /// </summary>
        protected PublicAppUsingDeviceCodeFlow tokenAcquisitionHelper;

        /// <summary>
        /// Gets or sets the Scopes
        /// Scopes to request access to the protected Web API (here Microsoft Graph)
        /// </summary>
        private static string[] Scopes { get; set; } = new string[] { "https://vault.azure.net/.default" };

        /// <summary>
        /// Defines the authenticationResult
        /// </summary>
        private AuthenticationResult authenticationResult;

        /// <summary>
        /// The GetToken
        /// </summary>
        /// <param name="clipboard">The clipboard<see cref="bool"/></param>
        /// <param name="emailRecipents">The emailRecipents<see cref="string"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
        public async Task<AuthenticationResult> GetToken(bool clipboard, string emailRecipents)
        {
            this.authenticationResult = await this.tokenAcquisitionHelper.AcquireATokenFromCacheOrDeviceCodeFlowAsync(Scopes, clipboard, emailRecipents).ConfigureAwait(false);
            if (this.authenticationResult != null)
            {
                // string idToken = this.authenticationResult.IdToken;
                return this.authenticationResult;
            }

            return null;
        }

        /// <summary>
        /// The DisplaySignedInAccount
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string DisplaySignedInAccount()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string message = $"{this.authenticationResult.Account.Username} successfully signed-in";
            Console.WriteLine(message);
            return message;
        }
    }
}
