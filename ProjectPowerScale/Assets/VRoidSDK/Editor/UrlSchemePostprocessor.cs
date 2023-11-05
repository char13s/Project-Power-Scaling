using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using UnityEngine;
using Pixiv.VroidSdk.Legacy;

namespace Pixiv.VroidSdk.Editor
{
    /// <summary>
    /// Unity iOSビルド後にUrlSchemeを追加する為のPostprocessor
    /// </summary>
    public class UrlSchemePostprocessor
    {
        /// <summary>
        /// ビルド後の処理
        /// </summary>
        [PostProcessBuild(1)]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS)
            {
                AddIOSUrlScheme(path);
            }
        }

        /// <summary>
        /// URLスキームを追加します
        /// </summary>
        /// <param name="path"> 出力先のパス </param>
        public static void AddIOSUrlScheme(string path)
        {
#if UNITY_IOS
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();

            // 読み込み
            plist.ReadFromFile(plistPath);

            PlistElementArray urlTypeArray = plist.root.CreateArray("CFBundleURLTypes");
            PlistElementDict urlTypeDict = urlTypeArray.AddDict();
            PlistElementArray urlSchemeArray = urlTypeDict.CreateArray("CFBundleURLSchemes");

            var cfg = UrlSchemeEditTool.GetUrlScheme();
            if (cfg.IosUrlScheme == null)
            {
                return;
            }

            var urlScheme = cfg.IosUrlScheme;
            var Validator = new UriSchemeValidator(urlScheme);
            if (!Validator.Validate())
            {
                Debug.LogError("iOSのURLスキーマが不正なのでURLスキーマの追加処理をスキップしました");
                return;
            }

            Uri uri;
            try
            {
                uri = new Uri(urlScheme);
            }
            catch (UriFormatException)
            {
                return;
            }

            // URLスキームを追加
            urlSchemeArray.AddString(uri.Scheme);

            // 書き込み
            plist.WriteToFile(plistPath);
#endif
        }
    }
}
