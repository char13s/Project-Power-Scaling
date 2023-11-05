using System.Collections.Generic;
using Pixiv.VroidSdk.Networking;

namespace Pixiv.VroidSdk.Browser
{
    /// <summary>
    /// 404 Not Foundのレスポンス内容
    /// </summary>
    public class NotFoundResponse : IHttpResponse
    {
        private static readonly string s_text = @"
        <!doctype html>
        <html lang=""en"">
            <head>
                <meta charSet=""utf-8"" />
                <title>VRoid SDK</title>
            </head>
            <body>
                404 Not Found
            </body>
        </html>
";
        /// <inheritdoc cref="IHttpResponse.Text"/>
        public string Text { get { return s_text; } }

        /// <inheritdoc cref="IHttpResponse.Data"/>
        public byte[] Data { get { return System.Text.Encoding.UTF8.GetBytes(Text); } }

        /// <inheritdoc cref="IHttpResponse.IsNetworkError"/>
        /// <value>常に<c>false</c></value>
        public bool IsNetworkError { get; } = false;

        /// <inheritdoc cref="IHttpResponse.IsHttpError"/>
        /// <value>常に<c>true</c></value>
        public bool IsHttpError { get; } = true;

        /// <inheritdoc cref="IHttpResponse.RawErrorMessage"/>
        public string RawErrorMessage { get { return Text; } }

        /// <inheritdoc cref="IHttpResponse.StatusCode"/>
        /// <value>常に<c>404</c></value>
        public int StatusCode { get; } = 404;

        /// <inheritdoc cref="IHttpResponse.ResponseHeaders"/>
        public Dictionary<string, string> ResponseHeaders { get; } = new Dictionary<string, string>();
    }
}
