using System;
using System.Net;

using Utilities.Common.Helpers.Setup;

namespace Utilities.Common.Helpers.Http
{
    /// <summary>
    /// Defines the <see cref="UrlChecker" />.
    /// </summary>
    public static class UrlChecker
    {
        /// <summary>
        /// The UrlHttpStatus.
        /// </summary>
        /// <param name="urlToCheck">The urlToCheck<see cref="Uri"/>.</param>
        /// <returns>The <see cref="HttpStatusCode"/>.</returns>
        public static HttpStatusCode UrlHttpStatus(Uri urlToCheck)
        {
            FunctionsAssemblyResolver.RedirectAssembly();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToCheck);
            request.Timeout = 15000;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(WebException))
                {
                    WebException tempException = ex as WebException;
                    if (tempException.Status == WebExceptionStatus.ProtocolError &&
                   tempException.Response != null)
                    {
                        HttpWebResponse resp = (HttpWebResponse)tempException.Response;
                        return resp.StatusCode == HttpStatusCode.Forbidden ? HttpStatusCode.Forbidden : resp.StatusCode;
                    }
                    else
                    {
                        return HttpStatusCode.BadRequest;
                    }
                }

                if (ex.GetType() == typeof(OperationCanceledException))
                {
                    return HttpStatusCode.BadRequest;
                }
                else
                {
                    throw;
                }
            }

            return response.StatusCode;
        }
    }
}
