using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Pixiv.VroidSdk.Oauth.DataModel;
using Pixiv.VroidSdk.Unity.Oauth.Legacy;
using Pixiv.VroidSdk.Unity.Environments;

namespace Pixiv.VroidSdk.Legacy
{
    /// <summary>
    /// VRoid SDKを利用するための設定
    /// </summary>
    [Obsolete("SDKConfiguration is deprecated. Please use Credential")]
    public partial class SDKConfiguration : ScriptableObject
    {
        /// <summary>
        /// 認可スコープの種類
        /// </summary>
        [Serializable]
        public partial class ScopeKind
        {
            [Tooltip("標準で使えるAPIを実行できます。\n(例) キャラクターモデルのダウンロード")]
            [SerializeField]
            public bool Default;
            [Tooltip("ハートに関する機能が使えるAPIを実行できます。\n(例) ハートをモデルにつける")]
            [SerializeField]
            public bool Heart;
        }

#pragma warning disable 0649
        [Tooltip("OAuthのアプリケーションIDを設定します。\nアプリケーション管理ページからOAuthの連携アプリケーションを作成し、作成したApplicationIdを入力してください")]
        [FormerlySerializedAs("ApplicationId")]
        [SerializeField]
        private string _applicationId;

        [Tooltip("OAuthのシークレットキーを設定します。\nアプリケーション管理ページからOAuthの連携アプリケーションを作成し、作成したSecretを入力してください")]
        [FormerlySerializedAs("Secret")]
        [SerializeField]
        private string _secret;
#pragma warning restore 0649

        /// <summary>
        /// Androidで認可コードを受け取る際に使用するURLスキーム
        /// </summary>
        [Tooltip("Android用のURLスキームを設定します。\nOAuthの連携アプリケーション作成画面のリダイレクトURIで設定した項目を入力します。\nリダイレクトURIにURLスキーマを設定していない場合は空文字にしてください")]
        public string AndroidUrlScheme;
        /// <summary>
        /// iOSで認可コードを受け取る際に使用するURLスキーム
        /// </summary>
        [Tooltip("iOS用のURLスキームを設定します。\nOAuthの連携アプリケーション作成画面のリダイレクトURIで設定した項目を入力します。\nリダイレクトURIにURLスキーマを設定していない場合は空文字にしてください")]
        public string IOSUrlScheme;
        /// <summary>
        /// 認可スコープの種類
        /// </summary>
        [Tooltip("API実行用のスコープを設定します。このスコープにより使えるAPIを変更できます")]
        public ScopeKind Scope;

        /// <summary>
        /// VRoid Hubアプリケーションのメタ情報を取得する
        /// </summary>
        /// <value>メタ情報</value>
        public AuthenticateMetaData AuthenticateMetaData
        {
            get
            {
                if (_metaData == null)
                {
#if !UNITY_EDITOR && UNITY_IOS
                    IAuthenticateSession session = new AuthenticateSessionIOS();
#else
                    IAuthenticateSession session = new AuthenticateSessionDefault();
#endif
                    _metaData = new AuthenticateMetaData(session, GetConfig());
                }

                return _metaData;
            }
        }
        private AuthenticateMetaData _metaData = null;

        /// <summary>
        /// スコープ設定を元に、連結したスコープパラメータを取得する
        /// </summary>
        /// <returns>空文字で連結されたスコープパラメータ</returns>
        public string JoinScope()
        {
            var joinedScope = GetScopes().Aggregate((a, b) => a + " " + b);
            return joinedScope.TrimEnd();
        }

        /// <summary>
        /// <inheritdoc cref="ISdkConfig"/>を作成する
        /// </summary>
        /// <returns>作成した<see cref="ISdkConfig"/></returns>
        public ISdkConfig GetConfig()
        {
            var credential = new Credential
            {
                ApplicationId = _applicationId.Trim(),
                Secret = _secret.Trim(),
                RedirectUri = "urn:ietf:wg:oauth:2.0:oob",
                IosUrlScheme = IOSUrlScheme,
                AndroidUrlScheme = AndroidUrlScheme,
                Scope = GetScopes().ToList()
            };

#if !UNITY_EDITOR && UNITY_IOS
            return new IosConfig(credential);
#elif !UNITY_EDITOR && UNITY_ANDROID
            return new AndroidConfig(credential);
#else
            return new DefaultConfig(credential);
#endif
        }

        private System.Collections.Generic.IEnumerable<string> GetScopes()
        {
            var typeInfo = typeof(ScopeKind).GetFields();
            return typeInfo.Select<FieldInfo, string>((props, _) =>
            {
                if ((bool)props.GetValue(Scope) == true)
                {
                    return props.Name.ToLower();
                }
                else
                {
                    return "";
                }
            });
        }
    }
}
