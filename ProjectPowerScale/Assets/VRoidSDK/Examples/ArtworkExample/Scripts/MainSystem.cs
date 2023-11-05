﻿using System.Collections.Generic;
using VRoidSDK.Examples.Core.Localize;
using UnityEngine;
using UnityEngine.UI;

namespace VRoidSDK.Examples.ArtworkExample
{
    public class MainSystem : ArtworkExampleEventHandler
    {
#pragma warning disable 0649
        [SerializeField] private Text _showArtworksText;
        [SerializeField] private Text _createArtworkText;
#pragma warning restore 0649

        public override void OnLangChanged(Translator.Locales locale)
        {
            switch (locale)
            {
                case Translator.Locales.JA:
                    _showArtworksText.text = "写真一覧";
                    _createArtworkText.text = "写真を投稿";
                    break;
                case Translator.Locales.EN:
                    _showArtworksText.text = "Your media";
                    _createArtworkText.text = "Upload media";
                    break;
                default:
                    break;
            }
        }
    }
}
