
namespace Pixiv.VroidSdk.Decorator
{
    /// <summary>
    /// 任意の文字列をUnityEngine.UI.Text向けに装飾するためのクラス
    /// </summary>
    public sealed class TextDecorator
    {
        private string _baseText;

        /// <summary>
        /// 装飾結果のテキストを取得する
        /// </summary>
        /// <value>装飾されたテキスト</value>
        public string Text
        {
            get
            {
                return _baseText;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="baseText">装飾対象のテキスト</param>
        public TextDecorator(string baseText)
        {
            _baseText = baseText;
        }

        /// <summary>
        /// 文字列にカラーコードで色をつける
        /// </summary>
        /// <param name="colorKey">カラーコード</param>
        /// <returns>装飾後のdecorator</returns>
        public TextDecorator Color(string colorKey)
        {
            _baseText = string.Format("<color={0}>{1}</color>", colorKey, _baseText);
            return this;
        }

        /// <summary>
        /// 太字にする
        /// </summary>
        /// <returns>装飾後のdecorator</returns>
        public TextDecorator Bold()
        {
            _baseText = string.Format("<b>{0}</b>", _baseText);
            return this;
        }
    }
}
