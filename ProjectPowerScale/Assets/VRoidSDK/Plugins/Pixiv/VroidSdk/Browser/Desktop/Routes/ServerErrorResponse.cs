using System.Collections.Generic;
using System;
using System.IO;
using Pixiv.VroidSdk.Networking;

namespace Pixiv.VroidSdk.Browser
{
    /// <summary>
    /// HTTPリクエスト処理中にハンドルしていない例外がスローされた時のレスポンス
    /// </summary>
    public class ServerErrorResponse : IHttpResponse
    {
        private static readonly string s_text = @"
        <!doctype html>
        <html lang=""en"">
            <head>
                <meta charSet=""utf-8"" />
                <title>VRoid SDK</title>
            </head>
            <body>
                500 Server Error
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
        /// <value>常に<c>500</c></value>
        public int StatusCode { get; } = 500;

        /// <summary>
        /// HTTPレスポンスヘッダ
        /// </summary>
        public Dictionary<string, string> ResponseHeaders { get; } = new Dictionary<string, string>();
    }
}
