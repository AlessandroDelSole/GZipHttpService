using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GZipHttpService
{
    public class HttpGZipClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            var response = await base.SendAsync(request, cancellationToken);
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                await response.Content.LoadIntoBufferAsync();
                response.Content = new HttpGZipContent(await response.Content.ReadAsStreamAsync());
            }
            return response;
        }
        public override bool SupportsAutomaticDecompression => true;
    }
}
