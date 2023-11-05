using System;
using UnityEngine;

namespace Pixiv.VroidSdk.Editor
{
    internal class VroidSdkReadme : ScriptableObject
    {
        public Texture2D icon;
        public string title;
        public Section[] sections;

        [Serializable]
        public class Section
        {
            public string heading, text, linkText, url;
        }
    }
}
