using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Services.AppAuthentication;

using System;

using Utilities.Common.Helpers.Setup;

namespace Utilities.Common.Helpers.Secrets
{
    /// <summary>
    /// Defines the <see cref="KeyvaultLoader" />.
    /// </summary>
    public static class KeyvaultLoader
    {
        /// <summary>
        /// Defines the keyVaultClient.
        /// </summary>
        private static KeyVaultClient keyVaultClient;

        /// <summary>
        /// Defines the keyVaultEndpoint.
        /// </summary>
        private static string keyVaultEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyvaultLoader"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint<see cref="string"/>.</param>
        /// <param name="authFile">The authFile<see cref="string"/>.</param>
        public static void Setup(string endpoint = null, string authFile = null)
        {
            keyVaultEndpoint = endpoint;

            if (authFile != null && System.IO.File.Exists(authFile))
            {
                AzureCredentials credentials = SdkContext.AzureCredentialsFactory.FromFile(authFile);
                _ = Azure
                    .Configure()
                    .Authenticate(credentials)
                    .WithDefaultSubscription();

                keyVaultClient = new KeyVaultClient(credentials);
            }
        }

        /// <summary>
        /// The LoadKeyVault.
        /// </summary>
        private static void LoadKeyVault()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(
                    azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        /// <summary>
        /// The GetSecret.
        /// </summary>
        /// <param name="secretname">The secretname<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetSecret(string secretname)
        {
            if (secretname is null)
            {
                throw new ArgumentNullException(nameof(secretname));
            }

            // Attempt to get from azure devops keyvault task.
            string temp = null;

            try
            {
                if (string.IsNullOrEmpty(temp))
                {
                    if (keyVaultClient == null)
                    {
                        LoadKeyVault();
                    }

                    temp = keyVaultClient.GetSecretAsync(keyVaultEndpoint, secretname).Result.Value;
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                throw;
            }

            return temp;
        }
    }
}
