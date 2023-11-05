using System;
using Pixiv.VroidSdk.Unity.Oauth.Legacy;
using UnityEngine;

namespace Pixiv.VroidSdk.Legacy
{
    [Obsolete("AuthenticateSessionDefault is deprecated. Please use BrowserProvider")]
    internal class AuthenticateSessionDefault : IAuthenticateSession
    {
        public void OpenURL(string url, string urlScheme)
        {
            Application.OpenURL(url);
        }

        public void CleanUp()
        {
        }
    }
}
