using System;
using Pixiv.VroidSdk.Oauth;
using UnityEngine;

namespace Pixiv.VroidSdk.Browser
{
    /// <summary>
    /// 認可コードの登録に失敗したときにスローされる例外
    /// </summary>
    public class RegisterCodeFailedException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public RegisterCodeFailedException(string message) : base(message) { }
    }

    /// <summary>
    /// 認可コードの登録をキャンセルしたときにスローされる例外
    /// </summary>
    public class RegisterCodeCancelException : Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public RegisterCodeCancelException(string message) : base(message) { }
    }

    internal sealed class DefaultBrowser : IManualCodeRegistrable
    {
        private Action<string> _onRegistered;

        public void OpenBrowserWindow(string url, Action<string> onRegisterCodeReceived, Action<Exception> onFailed)
        {
            _onRegistered = onRegisterCodeReceived;
            Application.OpenURL(url);
        }

        public void CleanUp() { }

        public void OnRegisterCode(string authCode)
        {
            _onRegistered?.Invoke(authCode);
        }
    }
}
