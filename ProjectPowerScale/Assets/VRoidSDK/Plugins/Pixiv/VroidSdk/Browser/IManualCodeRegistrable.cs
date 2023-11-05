using Pixiv.VroidSdk.Oauth;

namespace Pixiv.VroidSdk.Browser
{
    /// <summary>
    /// ブラウザから認可コードをコピーして登録するインターフェース
    /// </summary>
    public interface IManualCodeRegistrable : IBrowserControllable
    {
        /// <summary>
        /// 認可コードを登録する際に呼び出される
        /// </summary>
        /// <param name="code">認可コード文字列</param>
        void OnRegisterCode(string code);
    }
}
