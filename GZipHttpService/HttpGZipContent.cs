using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GZipHttpService
{
    internal sealed class HttpGZipContent : HttpContent
    {
        private readonly GZipStream _gZipStream;

        public HttpGZipContent(Stream deflatedStream)
        {
            _gZipStream = new GZipStream(deflatedStream, CompressionMode.Decompress);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return _gZipStream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _gZipStream.BaseStream.Length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            _gZipStream.Dispose();
            base.Dispose(disposing);
        }
    }
}