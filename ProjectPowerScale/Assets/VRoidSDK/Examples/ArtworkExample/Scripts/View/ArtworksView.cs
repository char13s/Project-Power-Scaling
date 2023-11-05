﻿using System.Collections.Generic;
using VRoidSDK.Examples.Core.View.Parts;
using UnityEngine;
using VRoidSDK.Examples.Core.View;
using Pixiv.VroidSdk.Api.DataModel;

namespace VRoidSDK.Examples.ArtworkExample.View
{
    public class ArtworksView : BaseView
    {
#pragma warning disable 0649
        [SerializeField] private Routes _routes;
#pragma warning restore 0649

        public VerticalScrollGroup userArtworksScrollRoot;
        public LoadImage userArtworksUserIcon;
        public Button seeMoreButton;

        public void SetUserIcon(WebImage icon)
        {
            userArtworksUserIcon.Load(icon);
        }

        public void SetArtworkThumbnails(List<Artwork> artworks)
        {
            userArtworksScrollRoot.Insert<Artwork, ArtworkThumbnail>(artworks, (artwork) =>
            {
                _routes.ShowArtwork(artwork.Artwork.id);
            });
        }
    }
}
