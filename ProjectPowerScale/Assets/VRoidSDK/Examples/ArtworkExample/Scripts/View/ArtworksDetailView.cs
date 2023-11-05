using VRoidSDK.Examples.Core.View.Parts;
using System.Collections.Generic;
using UnityEngine;
using VRoidSDK.Examples.Core.View;
using Pixiv.VroidSdk.Api.DataModel;

namespace VRoidSDK.Examples.ArtworkExample.View
{
    public class ArtworksDetailView : BaseView
    {
        public VerticalScrollGroup artworkMediumScrollRoot;
        public Button closeButton;

        public void SetArtworkMedia(List<ArtworkMedium> media)
        {
            artworkMediumScrollRoot.Insert<ArtworkMedium, ArtworkMediumImage>(media, null);
        }
    }
}
