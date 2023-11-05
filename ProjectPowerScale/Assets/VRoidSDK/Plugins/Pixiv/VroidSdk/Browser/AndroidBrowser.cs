using System;
using UnityEngine;

namespace Pixiv.VroidSdk.Browser
{
#if !UNITY_EDITOR && UNITY_ANDROID
    internal sealed class AndroidBrowser : MonoBehaviour, IManualCodeRegistrable
    {
        private Action<string> _onRegistered;
        private Action<Exception> _onFailed;

        public string State { get; set; }
        public string RedirectUri { get; set; }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                return;
            }

            using(AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using(AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (activity == null)
                {
                    _onFailed?.Invoke(new RegisterCodeFailedException("currentActivity is null"));
                    return;
                }

                using (AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent"))
                {
                    if (intent == null)
                    {
                        _onFailed?.Invoke(new RegisterCodeFailedException("getIntent is null"));
                        return;
                    }

                    using (AndroidJavaObject intentUri = intent.Call<AndroidJavaObject>("getData"))
                    {

                        if (intentUri == null)
                        {
                            _onFailed?.Invoke(new RegisterCodeFailedException("getData is null"));
                            return;
                        }

                        var appUrl = intentUri.Call<string>("toString");
                        try
                        {
                            var query = UrlParser.ParseAuthCode(appUrl, RedirectUri);
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
                }
            }
        }

        public void OpenBrowserWindow(string url, Action<string> onRegisterCodeReceived, Action<Exception> onFailed)
        {
            _onRegistered = onRegisterCodeReceived;
            _onFailed = onFailed;
            Application.OpenURL(url);
        }

        public void CleanUp() { }

        public void OnRegisterCode(string authCode)
        {
            _onRegistered?.Invoke(authCode);
        }
    }
#endif
}
