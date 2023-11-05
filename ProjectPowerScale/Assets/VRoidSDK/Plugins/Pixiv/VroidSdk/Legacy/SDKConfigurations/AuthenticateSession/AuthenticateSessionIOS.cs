using System;
using System.Runtime.InteropServices;
using Pixiv.VroidSdk.Unity.Oauth.Legacy;
using UnityEngine;

namespace Pixiv.VroidSdk.Legacy
{
#if !UNITY_EDITOR && UNITY_IOS
    [Obsolete("AuthenticateSessionIOS is deprecated. Please use BrowserProvider")]
    internal class AuthenticateSessionIOS : IAuthenticateSession
    {
        [DllImport("__Internal")]
        private static extern void OpenBrowserWindow(string url, string urlScheme);

        [DllImport("__Internal")]
        private static extern void ReleaseSession();

        public void OpenURL(string url, string urlScheme)
        {
            OpenBrowserWindow(url, urlScheme);
        }

        public void CleanUp()
        {
            ReleaseSession();
        }
    }
#endif
}
