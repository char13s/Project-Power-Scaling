using VRoidSDK.Examples.Core.Model;
using UnityEngine;
using Pixiv.VroidSdk.Api.DataModel;

namespace VRoidSDK.Examples.ArtworkExample.Model
{
    public class ArtworkDetailModel : ApplicationModel
    {
        public ArtworkDetail ArtworkDetail { get; set; }

        public ArtworkDetailModel()
        {
            ArtworkDetail = null;
        }
    }
}
