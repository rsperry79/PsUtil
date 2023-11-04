using Polly;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Common.Helpers.Http
{
    /// <summary>
    /// Defines the <see cref="HttpRetryMessageHandler" />.
    /// </summary>
    public class HttpRetryMessageHandler : DelegatingHandler
    {
        /// Initializes a new instance of the <see cref="HttpRetryMessageHandler"/> class.
        /// </summary>
        /// <param name="handler">The handler<see cref="HttpClientHandler"/></param>
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRetryMessageHandler"/> class.
        /// </summary>
        /// <param name="handler">The handler<see cref="HttpClientHandler"/>.</param>
        public HttpRetryMessageHandler(HttpClientHandler handler) : base(handler)
        {
        }

        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequestMessage"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Policy
        .Handle<HttpRequestException>()
        .Or<TaskCanceledException>()
        .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(3, retryAttempt)))
        .ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
