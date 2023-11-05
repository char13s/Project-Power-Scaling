using Pixiv.VroidSdk.Oauth;
using Pixiv.VroidSdk.Browser;
using Pixiv.VroidSdk.Logger;
using Pixiv.VroidSdk.Oauth.DataModel;
using UnityEngine;

namespace Pixiv.VroidSdk
{
    /// <summary>
    /// 認可コードを手動で登録する<see cref="IManualCodeRegistrable"/>を作成する
    /// </summary>
    public static class BrowserProvider
    {
        /// <summary>
        /// 認可コードを手動で登録する<see cref="IManualCodeRegistrable"/>を作成する
        /// </summary>
        /// <param name="client">HTTPリクエストを行うクライアント</param>
        /// <param name="config">VRoid SDKの利用設定</param>
        /// <returns>認可コードを手動でコピーする<see cref="IManualCodeRegistrable"/></returns>
        public static IManualCodeRegistrable Create(Client client, ISdkConfig config)
        {
            if (config.IsManualLogin)
            {
                return new DefaultBrowser();
            }

#if !UNITY_EDITOR && UNITY_IOS
            var gameObjectName = "BrowserAuthorize";
            var oldInstance = GameObject.Find(gameObjectName);
            if (oldInstance != null)
            {
                GameObject.Destroy(oldInstance);
            }

            var instanceGo = new GameObject(gameObjectName);
            GameObject.DontDestroyOnLoad(instanceGo);

            var ba = instanceGo.AddComponent<IosBrowser>();
            ba.State = client.State;
            ba.RedirectUri = config.Credential.IosUrlScheme;
#elif !UNITY_EDITOR && UNITY_ANDROID
            var gameObjectName = "BrowserAuthorize";
            var oldInstance = GameObject.Find(gameObjectName);
            if (oldInstance != null)
            {
                GameObject.Destroy(oldInstance);
            }

            var instanceGo = new GameObject(gameObjectName);
            GameObject.DontDestroyOnLoad(instanceGo);

            var ba = instanceGo.AddComponent<AndroidBrowser>();
            ba.State = client.State;
            ba.RedirectUri = config.Credential.AndroidUrlScheme;
#elif UNITY_EDITOR_OSX || (!UNITY_EDITOR && UNITY_STANDALONE_OSX)
            var ba = new MacosDesktopBrowser(client.State, config.RedirectUri);
#elif UNITY_EDITOR_WIN || (!UNITY_EDITOR && UNITY_STANDALONE_WIN)
            var ba = new WindowsDesktopBrowser(client.State, config.RedirectUri);
#else
            var ba = new DefaultBrowser();
#endif
            return ba;
        }
    }
}
