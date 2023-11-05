using System.Collections.Generic;
using System.IO;
using Pixiv.VroidSdk.Networking;

namespace Pixiv.VroidSdk.Browser
{
    internal sealed class IndexHtmlResponse : IHttpResponse
    {
        public string Text { get; }

        public byte[] Data
        {
            get
            {
                return System.Text.Encoding.UTF8.GetBytes(Text);
            }
        }

        public bool IsNetworkError { get; } = false;

        public bool IsHttpError { get; } = false;

        public string RawErrorMessage { get; } = "";

        public int StatusCode { get; } = 301;

        public Dictionary<string, string> ResponseHeaders { get; }

        public IndexHtmlResponse(string redirectTo)
        {
            ResponseHeaders = new Dictionary<string, string>();
            ResponseHeaders.Add("Location", redirectTo);
        }
    }
}
