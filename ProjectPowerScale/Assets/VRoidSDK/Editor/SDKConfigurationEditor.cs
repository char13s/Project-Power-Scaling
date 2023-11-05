using UnityEditor;
using Pixiv.VroidSdk.Legacy;
using Pixiv.VroidSdk.Editor;

#pragma warning disable 0618
[CustomEditor(typeof(SDKConfiguration), true)]
#pragma warning restore 0618
public sealed class SDKConfigurationInspector : Editor
{
#pragma warning disable 0618
    private SDKConfiguration _configuration;
#pragma warning restore 0618

    private void OnEnable()
    {
        //SDKConfigurationを取得
#pragma warning disable 0618
        _configuration = (SDKConfiguration)target;
#pragma warning restore 0618
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var isError = false;
        var authenticateMetaDataValidator = new AuthenticateMetaDataValidator(_configuration);
        var iOSUriSchemeValidator = new IOSUriSchemeValidator(_configuration);
        var androidUriSchemeValidator = new AndroidUriSchemeValidator(_configuration);

        if (!authenticateMetaDataValidator.Validate())
        {
            EditorGUILayout.HelpBox("アプリケーションID, またはシークレットキーが空欄になっています", MessageType.Error);
            isError = true;
        }

        if (!iOSUriSchemeValidator.Validate())
        {
            EditorGUILayout.HelpBox("iOSのURLスキーマが不正です。\nURLとして認識できないもの、または `my-vroidsdk-app` をスキームに利用するURLは使用できません", MessageType.Error);
            isError = true;
        }

        if (!androidUriSchemeValidator.Validate())
        {
            EditorGUILayout.HelpBox("AndroidのURLスキーマが不正です。\nURLとして認識できないもの、または `my-vroidsdk-app` をスキームに利用するURLは使用できません", MessageType.Error);
            isError = true;
        }

        if (!isError)
        {
            EditorGUILayout.HelpBox("全ての設定が正常です", MessageType.Info);
        }
    }
}
