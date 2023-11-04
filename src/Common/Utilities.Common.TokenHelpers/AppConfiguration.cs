using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace Utilities.Common.TokenHelpers
{
    /// <summary>
    /// Description of the configuration of an AzureAD public client application (desktop/mobile application). This should
    /// match the application registration done in the Azure portal
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// Gets or sets the PublicClientApplicationOptions
        /// Authentication options
        /// </summary>
        public PublicClientApplicationOptions PublicClientApplicationOptions { get; set; }

        /// <summary>
        /// Gets or sets the ClientSecret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Reads the configuration from a json file
        /// </summary>
        /// <param name="path">Path to the configuration json file</param>
        /// <returns>SampleConfiguration as read from the json file</returns>
        public static AppConfiguration ReadFromJsonFile(string path)
        {
            // .NET configuration
            IConfigurationRoot configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder()
             .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile(path);

            configuration = builder.Build();
            // Read the auth and graph endpoint config
            AppConfiguration config = new AppConfiguration()
            {
                PublicClientApplicationOptions = new PublicClientApplicationOptions()
            };
            configuration.Bind("Authentication", config.PublicClientApplicationOptions);
            return config;
        }
    }
}
