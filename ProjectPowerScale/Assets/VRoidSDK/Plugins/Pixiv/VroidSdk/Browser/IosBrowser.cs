using System;
using System.Runtime.InteropServices;
using Pixiv.VroidSdk.Oauth;
using UnityEngine;

namespace Pixiv.VroidSdk.Browser
{
#if !UNITY_EDITOR && UNITY_IOS
    internal sealed class IosBrowser : MonoBehaviour, IManualCodeRegistrable
    {
        private Action<string> _onRegistered;
        private Action<Exception> _onFailed;

        public string State { get; set; }
        public string RedirectUri { get; set; }

        public void OpenBrowserWindow(string url, Action<string> onRegisterCodeReceived, Action<Exception> onFailed)
        {
            _onRegistered = onRegisterCodeReceived;
            _onFailed = onFailed;
            Pixiv.VroidSdk.Native.IosPlugin.OpenBrowserWindow(url, RedirectUri);
        }

        public void CleanUp()
        {
            Pixiv.VroidSdk.Native.IosPlugin.ReleaseSession();
        }

        /// <summary>
        /// URLスキーマによりリダイレクトされたときに呼び出されるメソッド.
        /// パスに埋め込まれている認可コードを取り出して、登録を行う
        /// </summary>
        /// <param name="url">リダイレクトURL</param>
        /// <remarks>
        /// このメソッドはiOSのネイティブプラグインからOAuthの認証コードを受け取る時にも利用される
        /// </remarks>
        public void OnOpenUrl(string url)
        {
            try
            {
                var query = UrlParser.ParseAuthCode(url, RedirectUri);
                if (query.State != State)
                {
                    _onFailed?.Invoke(new RegisterCodeFailedException("invalid state"));
                    return;
                }

                if (string.IsNullOrEmpty(query.AuthCode))
                {
                    _onFailed?.Invoke(new RegisterCodeFailedException("authCode is null or empty"));
                    return;
                }

                _onRegistered?.Invoke(query.AuthCode);
            } catch(InvalidCallbackUrlException ex)
            {
                _onFailed?.Invoke(ex);
            }
        }

        /// <summary>
        /// ブラウザ認証がキャンセルされたときに呼ばれるメソッド
        /// </summary>
        /// <param name="_message">メッセージ</param>
        /// <remarks>
        /// このメソッドはiOSのネイティブプラグインからOAuthの認証コードがキャンセルされたときに利用される
        /// </remarks>
        public void OnCancelAuthorize(string _message)
        {
            _onFailed?.Invoke(new RegisterCodeCancelException("code cancel"));
        }

        public void OnRegisterCode(string authCode)
        {
            _onRegistered?.Invoke(authCode);
        }
    }
#endif
}
