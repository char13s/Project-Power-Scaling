using System.Runtime.InteropServices;
namespace Pixiv.VroidSdk.Native
{
#if !UNITY_EDITOR && UNITY_IOS
    /// <summary>
    /// iOSネイティブ機能を呼び出す
    /// </summary>
    public sealed class IosPlugin
    {
        [DllImport("__Internal")]
        /// <summary>
        /// ブラウザを開く
        /// </summary>
        /// <param name="url">開くページのURL</param>
        /// <param name="urlScheme">Callbackで戻るときのURL Schema</param>
        /// <remarks>
        /// iOSの各バージョンによって、それぞれURLを開く方式が違う
        /// iOS10以前: SFSafariViewController
        /// iOS11:    SFAuthenticationSession
        /// iOS12以降: ASWebAuthenticationSession
        /// </remarks>
        public static extern void OpenBrowserWindow(string url, string urlScheme);

        [DllImport("__Internal")]
        /// <summary>
        /// OpenBrowserWindowで使ったアンマネージドリソースを解放する
        /// </summary>
        public static extern void ReleaseSession();
    }
#endif
}
