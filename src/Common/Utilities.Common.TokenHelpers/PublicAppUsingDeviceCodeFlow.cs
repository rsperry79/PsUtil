using Microsoft.Identity.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Utilities.Common.Helpers.Setup;
using Utilities.Common.Helpers.Utilities;

namespace Utilities.Common.TokenHelpers
{
    /// <summary>
    /// Security token provider using the Device Code flow
    /// </summary>
    public class PublicAppUsingDeviceCodeFlow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublicAppUsingDeviceCodeFlow"/> class.
        /// </summary>
        /// <param name="app">The app<see cref="IPublicClientApplication"/></param>
        public PublicAppUsingDeviceCodeFlow(IPublicClientApplication app)
        {
            App = app;
        }

        /// <summary>
        /// Gets the App
        /// </summary>
        protected IPublicClientApplication App { get; private set; }

        /// <summary>
        /// Acquires a token from the token cache, or device code flow
        /// </summary>
        /// <param name="scopes">The scopes<see cref="IEnumerable{string}"/></param>
        /// <param name="clipboard">The clipboard<see cref="bool"/></param>
        /// <param name="emailRecipents">The emailRecipents<see cref="string"/></param>
        /// <returns>An AuthenticationResult if the user successfully signed-in, or otherwise <c>null</c></returns>
        public async Task<AuthenticationResult> AcquireATokenFromCacheOrDeviceCodeFlowAsync(IEnumerable<string> scopes, bool clipboard, string emailRecipents)
        {
            AuthenticationResult result = null;
            IEnumerable<IAccount> accounts = await App.GetAccountsAsync().ConfigureAwait(false);

            if (accounts.Any())
            {
                try
                {
                    // Attempt to get a token from the cache (or refresh it silently if needed)
                    result = await App.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                        .ExecuteAsync().ConfigureAwait(false);
                }
                catch (MsalUiRequiredException ex)
                {
                    Log.LogException(ex);
                }
            }

            // Cache empty or no token for account in the cache, attempt by device code flow
            result ??= await GetTokenForWebApiUsingDeviceCodeFlowAsync(scopes, clipboard, emailRecipents).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Gets an access token so that the application accesses the web api in the name of the user
        /// who signs-in on a separate device
        /// </summary>
        /// <param name="scopes">The scopes<see cref="IEnumerable{string}"/></param>
        /// <param name="clipboard">The clipboard<see cref="bool"/></param>
        /// <param name="emailRecipents">The emailRecipents<see cref="string"/></param>
        /// <returns>The <see cref="Task{AuthenticationResult}"/></returns>
        private async Task<AuthenticationResult> GetTokenForWebApiUsingDeviceCodeFlowAsync(IEnumerable<string> scopes, bool clipboard, string emailRecipents)
        {
            AuthenticationResult result;
            try
            {
                result = await App.AcquireTokenWithDeviceCode(
                    scopes,
                    deviceCodeCallback =>
                    {
                        // This will print the message on the console which tells the user where to go sign-in using
                        // a separate browser and the code to enter once they sign in.
                        // The AcquireTokenWithDeviceCodeAsync() method will poll the server after firing this
                        // device code callback to look for the successful login of the user via that browser.
                        // This background polling (whose interval and timeout data is also provided as fields in the
                        // deviceCodeCallback class) will occur until:
                        // * The user has successfully logged in via browser and entered the proper code
                        // * The timeout specified by the server for the lifetime of this code (typically ~15 minutes) has been reached
                        // * The developing application calls the Cancel() method on a CancellationToken sent into the method.
                        //   If this occurs, an OperationCanceledException will be thrown (see catch below for more details).
                        if (clipboard)
                        {
                            CoreUtils.OpenDeviceCode(deviceCodeCallback.VerificationUrl, deviceCodeCallback.UserCode);
                            Console.WriteLine($"Code interval is {deviceCodeCallback.Interval}");
                            Console.WriteLine(deviceCodeCallback.Message);
                            Console.WriteLine("Web page should have opened with the code in the clipboard.");
                        }
                        else
                        {
                            if (emailRecipents != null)
                            {
                                // needs to be setup, built in-line for a unmovable requirement.
                                var settings = new SmtpSettings()
                                {

                                    Code = "code",
                                    Host = "host",
                                    Port = 25,
                                    Username = "user-name"
                                };


                                SendEmail sender = new SendEmail(settings);
                                _ = sender.Send(emailRecipents, "AAD Notification", deviceCodeCallback.Message);
                            }

                            Console.WriteLine(deviceCodeCallback.Message);
                        }

                        return Task.FromResult(0);
                    }).ExecuteAsync().ConfigureAwait(false);
            }
            catch (MsalServiceException ex)
            {
                // Kind of errors you could have (in errorCode and ex.Message)
                string errorCode = ex.ErrorCode;
                Log.LogException(ex);
                // AADSTS50059: No tenant-identifying information found in either the request or implied by any provided credentials.
                // Mitigation: as explained in the message from Azure AD, the authoriy needs to be tenanted. you have probably created
                // your public client application with the following authorities:
                // https://login.microsoftonline.com/common or https://login.microsoftonline.com/organizations

                // AADSTS90133: Device Code flow is not supported under /common or /consumers endpoint.
                // Mitigation: as explained in the message from Azure AD, the authority needs to be tenanted

                // AADSTS90002: Tenant <tenantId or domain you used in the authority> not found. This may happen if there are
                // no active subscriptions for the tenant. Check with your subscription administrator.
                // Mitigation: if you have an active subscription for the tenant this might be that you have a typo in the
                // tenantId (GUID) or tenant domain name, update the

                // The issues above are typically programming / app configuration errors, they need to be fixed
                throw;
            }
            catch (OperationCanceledException)
            {
                // If you use an override with a CancellationToken, and call the Cancel() method on it, then this may be triggered
                // to indicate that the operation was cancelled.
                // See https://docs.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads
                // for more detailed information on how C# supports cancellation in managed threads.
                result = null;
            }
            catch (MsalClientException ex)
            {
                string errorCode = ex.ErrorCode;

                // Verification code expired before contacting the server
                // This exception will occur if the user does not manage to sign-in before a time out (15 mins) and the
                // call to `AcquireTokenWithDeviceCodeAsync` is not cancelled in between
                result = null;
            }

            return result;
        }
    }
}
