using System.Collections.Generic;

namespace Pixiv.VroidSdk.Localize
{
    /// <summary>
    /// 日本語表記のローカライズをするクラス
    /// </summary>
    public class Ja : En
    {
        private static readonly Dictionary<string, string> s_localeDictionary = new Dictionary<string, string>()
        {
            { Key.LicenseTypeOk, "OK" },
            { Key.LicenseTypeNg, "NG" },
            { Key.LicenseTypeNeed, "必要" },
            { Key.LicenseTypeNoNeed, "不要" },
            { Key.LicenseTypeProfit, "OK - ギフティング" },
            { Key.LicenseTypeNonProfit, "OK - 同人" },
            { Key.LicenseTypeNotSet, "未設定" },
            { Key.LicenseTextTitle, "モデルデータの利用条件" },
            { Key.LicenseTextCanUseAvatar, "アバターでの利用" },
            { Key.LicenseTextCanUseViolence, "暴力表現での利用" },
            { Key.LicenseTextCanUseSexuality, "性的表現での利用" },
            { Key.LicenseTextCanUseCorporateCommercial, "法人の商用利用" },
            { Key.LicenseTextCanUsePersonalCommercial, "個人の商用利用" },
            { Key.LicenseTextCanModify, "改変" },
            { Key.LicenseTextCanRedistribute, "再配布" },
            { Key.LicenseTextShowCredit, "クレジット表記" },
            { Key.LicenseTextFormat, "フォーマット" },
            { Key.LicenseTextVRM00, "VRM0.0" },
            { Key.LicenseTextVRM10, "VRM1.0" },
            { Key.Vrm10LicenseTextCanUseAvatar, "アバター利用" },
            { Key.Vrm10LicenseTextCanUseViolence, "暴力表現での利用" },
            { Key.Vrm10LicenseTextCanUseSexuality, "性的表現での利用" },
            { Key.Vrm10LicenseTextCanUseReligionPolitics, "宗教・政治目的での表現" },
            { Key.Vrm10LicenseTextCanUseAntisocialHatred, "反社会的・憎悪表現での利用" },
            { Key.Vrm10LicenseTextCanUseCorporate, "法人利用" },
            { Key.Vrm10LicenseTextCanUseCommercial, "商用利用" },
            { Key.Vrm10LicenseTextCanModify, "改変" },
            { Key.Vrm10LicenseTextCanRedistribute, "再配布" },
            { Key.Vrm10LicenseTextCanRedistributeModified, "改変物の再配布" },
            { Key.Vrm10LicenseTextShowCredit, "クレジット表記" },
        };

        /// <summary>
        /// 任意の文字列を受け取り日本語表記を出力する
        /// </summary>
        /// <param name="key">ローカライズのキー</param>
        /// <returns>日本語表記の文字列</returns>
        /// <remarks>
        /// ローカライズのキーがない場合、基底クラスにフォールバックする
        /// </remarks>
        public override string Get(string key)
        {
            if (s_localeDictionary.ContainsKey(key))
            {
                return s_localeDictionary[key];
            }

            return base.Get(key);
        }
    }
}
