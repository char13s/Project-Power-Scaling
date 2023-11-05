using System;
using System.Threading.Tasks;
using UnityEngine;
using Pixiv.VroidSdk.Networking.Transports;

namespace Pixiv.VroidSdk.Browser
{
#if UNITY_EDITOR_WIN || (!UNITY_EDITOR && UNITY_STANDALONE_WIN)
    internal sealed class WindowsDesktopBrowser : IManualCodeRegistrable
    {
        private Action<string> _onRegistered;
        private INetworkListener _listener;
        private Uri _redirectUri;
        private IntPtr _windowHandler;

        public WindowsDesktopBrowser(string state, string redirectUri)
        {
            _redirectUri = new Uri(redirectUri);
            var router = new Router((context) =>
            {
                context.Define("/", (request) =>
                {
                    var authCode = UrlParser.ParseAuthCode(request.Url.AbsoluteUri, redirectUri);

                    if (authCode.State == state)
                    {
                        OnRegisterCode(authCode.AuthCode);
                    }

                    return new IndexHtmlResponse("https://hub.vroid.com/sdk/auth_success");
                });
                context.Define("404", (_) => new NotFoundResponse());
                context.Define("500", (_) => new ServerErrorResponse());
            });

            _listener = new OnetimeHttpServer(router);
        }

        public void CleanUp()
        {
        }

        public void OnRegisterCode(string code)
        {
            if (_windowHandler != IntPtr.Zero)
            {
                Native.WindowsPlugin.SetApplicationFocus(_windowHandler);
            }
            _onRegistered?.Invoke(code);
        }

        public void OpenBrowserWindow(string url, Action<string> onRegisterCodeReceived, Action<Exception> onFailed)
        {
            _windowHandler = Native.WindowsPlugin.GetActiveWindow();
            _onRegistered = onRegisterCodeReceived;
            _listener.ListenAsync(_redirectUri).ContinueWith((result) =>
            {
                if (result.Exception != null)
                {
                    Debug.LogError(result.Exception.InnerException?.Message);
                    onFailed?.Invoke(result.Exception.InnerException);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Application.OpenURL(url);
        }
    }
#endif
}
