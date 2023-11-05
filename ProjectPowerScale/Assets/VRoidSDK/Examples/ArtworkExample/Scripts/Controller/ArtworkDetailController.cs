using System;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.ArtworkExample.Renderer;
using VRoidSDK.Examples.ArtworkExample.Model;

namespace VRoidSDK.Examples.ArtworkExample.Controller
{
    public class ArtworkDetailController : BaseController
    {
        private ApiController _api;
        private ArtworkDetailModel _model;

        public ArtworkDetailController(ApiController api)
        {
            _api = api;
            _model = new ArtworkDetailModel();
        }

        public void ShowArtwork(string artworkId, Action<IRenderer> onResponse)
        {
            CheckLogin(_api, onResponse, (account) =>
            {
                _api.GetArtworks(artworkId, (result) =>
                {
                    _model.Active = true;
                    _model.ArtworkDetail = result;
                    onResponse(new ArtworkDetailRenderer(_model));
                }, (error) =>
                {
                    _model.ApiError = error;
                    onResponse(new ApiErrorRenderer(_model));
                });
            });
        }

        public void HideArtwork(Action<IRenderer> onResponse)
        {
            _model.Active = false;
            _model.ArtworkDetail = null;
            onResponse(new ArtworkDetailRenderer(_model));
        }
    }
}
