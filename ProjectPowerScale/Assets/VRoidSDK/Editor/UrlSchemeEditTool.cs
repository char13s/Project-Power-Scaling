using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Pixiv.VroidSdk.Legacy;
using Pixiv.VroidSdk.Oauth.DataModel;
namespace Pixiv.VroidSdk.Editor
{
    public class UrlSchemeEditTool
    {
        public class UrlScheme
        {
            public string IosUrlScheme;
            public string AndroidUrlScheme;
        }

#pragma warning disable 0618
        public static SDKConfiguration LoadSDKConfiguration()
#pragma warning restore 0618
        {
            string[] paths = AssetDatabase.GetAllAssetPaths();
            for (int i = 0; i < paths.Length; ++i)
            {
#pragma warning disable 0618
                SDKConfiguration sdkConfiguration = AssetDatabase.LoadAssetAtPath<SDKConfiguration>(paths[i]);
#pragma warning restore 0618
                if (sdkConfiguration != null)
                {
                    return sdkConfiguration;
                }
            }
            return null;
        }

        public static UrlScheme GetUrlScheme()
        {
            var asset = Resources.Load<TextAsset>("credential.json");
            if (asset == null)
            {
                var cfg = LoadSDKConfiguration();
                return new UrlScheme
                {
                    IosUrlScheme = cfg?.IOSUrlScheme,
                    AndroidUrlScheme = cfg?.AndroidUrlScheme,
                };
            }

            var credential = Credential.FromJson(asset.text);

            return new UrlScheme
            {
                IosUrlScheme = credential.IosUrlScheme,
                AndroidUrlScheme = credential.AndroidUrlScheme,
            };
        }
    }
}
