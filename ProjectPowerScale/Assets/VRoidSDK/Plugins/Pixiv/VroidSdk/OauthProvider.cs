using System;
using System.Threading;
using Pixiv.VroidSdk.Oauth;
using Pixiv.VroidSdk.Networking.Drivers;
using Pixiv.VroidSdk.Unity.Environments;

namespace Pixiv.VroidSdk
{
    /// <summary>
    /// VRoid Hub連携を行う認可クライアントを作成する
    /// </summary>
    public static class OauthProvider
    {
        /// <summary>
        /// VRoid Hub連携を行う<see cref="Client"/>を作成する
        /// </summary>
        /// <param name="config">VRoid SDKを利用するための設定</param>
        /// <param name="driver"><see cref="Pixiv.VroidSdk.Networking.Connections.IHttpConnection"/>を生成できるオブジェクト</param>
        /// <returns>VRoid Hub連携を行う<see cref="Client"/></returns>
        public static Client CreateOauthClient(ISdkConfig config, IHttpConnectionDriver driver)
        {
            return new Client(config, driver);
        }

        /// <summary>
        /// VRoid Hub連携を行う<see cref="Client"/>を作成する
        /// </summary>
        /// <param name="context">コールバックを呼び出す際の<see cref="SynchronizationContext"/></param>
        /// <param name="config">VRoid SDKを利用するための設定</param>
        /// <returns>VRoid Hub連携を行う<see cref="Client"/></returns>
        public static Client CreateOauthClient(ISdkConfig config, SynchronizationContext context)
        {
            return CreateOauthClient(config, new HttpClientDriver(context));
        }

        /// <summary>
        /// プラットフォームに応じた<see cref="ISdkConfig"/>を作成する。
        /// </summary>
        /// <remarks>
        /// iOSでは<see cref="IosConfig"/>、Androidでは<see cref="AndroidConfig"/>、それ以外のプラットフォームでは<see cref="DefaultConfig"/>を返す
        /// </remarks>
        /// <param name="json">credential.json.bytesの内容</param>
        /// <returns>プラットフォームに応じた<see cref="ISdkConfig"/></returns>
        public static ISdkConfig CreateSdkConfig(string json)
        {
#if !UNITY_EDITOR && UNITY_IOS
            return new IosConfig(json);
#elif !UNITY_EDITOR && UNITY_ANDROID
            return new AndroidConfig(json);
#else
            return new DesktopConfig(json);
#endif
        }
    }
}
