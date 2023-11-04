#pragma warning disable SA1005
//using Microsoft.Identity.Client;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Runtime.InteropServices.ComTypes;
//using System.Threading.Tasks;

// Single line comments should begin with single space
//namespace Utilities.Common.TokenHelpers
// Single line comments should begin with single space
//{
//    public class EmailNotifier
//    {

//        public async static Task SendEmailNotification(string subject, string message, string recipents)
//        {
//            AppConfiguration config = AppConfiguration.ReadFromJsonFile("appsettings.json");
//            PublicClientApplicationOptions appConfig = config.PublicClientApplicationOptions;

//            string tenantId = config.PublicClientApplicationOptions.TenantId;
//            string clientId = config.PublicClientApplicationOptions.ClientId;
//            string resourceId = "https://outlook.office.com/";
//            string resourceUrl = "https://outlook.office.com/api/v2.0/users/v-risper@microsoft.com/sendmail"; //this is your on-behalf user's UPN
//            string authority = string.Format("https://login.windows.net/{0}", tenantId);
//            List<dynamic> toList = new List<dynamic>();
//            recipents.Split(',').ToList().ForEach(item =>
//            {
//                toList.Add(new { EmailAddress = new { Address = item } });
//            }
//            );
//            var itemPayload = new
//            {

//                Message = new
//                {
//                    Subject = subject,
//                    Body = new { ContentType = "Text", Content = message },
//                    ToRecipients = toList.ToArray()
//                }
//            };

//            AuthenticationContext authenticationContext = new AuthenticationContext(authority, false);

//            Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential clientCredential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(config.PublicClientApplicationOptions.ClientId, config.ClientSecret);
//            //get the access token to Outlook using the ClientAssertionCertificate
//            var authenticationResult = await authenticationContext.AcquireTokenAsync(resourceId, clientCredential);
//            string token = authenticationResult.AccessToken;

//            //initialize HttpClient for REST call
//            HttpClient client = new HttpClient();
//            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
//            client.DefaultRequestHeaders.Add("Accept", "application/json");

//            //setup the client post
//            HttpContent content = new StringContent(JsonConvert.SerializeObject(itemPayload));
//            //Specify the content type.
//            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json;odata=verbose");
//            HttpResponseMessage result = await client.PostAsync(resourceUrl, content);
//            if (result.IsSuccessStatusCode)
//            {
//                //email send successfully.
//                Console.WriteLine("Email sent successfully. ");
//            }
//            else
//            {
//                //email send failed. check the result for detail information from REST api.
//                Console.WriteLine("Email sent failed. Error: {0}", await result.Content.ReadAsStringAsync());
//            }

//        }
//    }
//}
#pragma warning restore SA1005