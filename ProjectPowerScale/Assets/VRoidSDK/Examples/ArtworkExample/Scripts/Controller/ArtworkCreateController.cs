using System;
using System.Collections.Generic;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.ArtworkExample.Renderer;
using VRoidSDK.Examples.ArtworkExample.Model;
using Pixiv.VroidSdk.Api.DataModel;
using Pixiv.VroidSdk.Api.Params;

namespace VRoidSDK.Examples.ArtworkExample.Controller
{
    public class ArtworkCreateController : BaseController
    {
        private ApiController _api;
        private ArtworkUploadModel _model;
        private string _httpHostUrl;

        public ArtworkCreateController(string httpHostUrl, ApiController api)
        {
            _api = api;
            _model = new ArtworkUploadModel();
            _httpHostUrl = httpHostUrl;
        }

        public void CreateArtwork(UnityEngine.Texture2D texture, bool isArchived, Action<IRenderer> onResponse)
        {
            CheckLogin(_api, onResponse, (account) =>
            {
                _model.Active = true;
                _model.ScreenShot = texture;
                _model.IsArchived = isArchived;
                onResponse(new ArtworkCreateRenderer(_model));
            });
        }

        public void HideCreateArtwork(Action<IRenderer> onResponse)
        {
            _model.Active = false;
            onResponse(new ArtworkCreateRenderer(_model));
        }

        public void UpdateCaption(string caption)
        {
            _model.Caption = caption;
        }

        public void UpdateAgeLimit(EnumAgeLimit limit, bool changeTo)
        {
            if (changeTo == false) return;
            _model.AgeLimit = limit;
        }

        public void UploadArtwork(Action<Uri> onUploaded, Action<IRenderer> onResponse)
        {
            CheckLogin(_api, onResponse, (_) =>
            {
                _model.UploadProgressActive = true;
                onResponse(new ArtworkCreateRenderer(_model));

                var png = UnityEngine.ImageConversion.EncodeToPNG(_model.ScreenShot);
                var param = new PostArtworkMediaImagesParams(png);

                _api.PostArtworkMediaImages(param, (medium) =>
                {
                    var artworkMedia = new List<string> { medium.id };
                    var artworksParams = new PostArtworksParams(_model.Caption, _model.AgeLimit, artworkMedia, _model.IsArchived);
                    _api.PostArtworks(artworksParams, (artworkDetail) =>
                    {
                        _model.Progress = 1.0f;
                        _model.UploadProgressActive = false;
                        _model.Active = false;
                        onResponse(new ArtworkCreateRenderer(_model));
                        onUploaded?.Invoke(ArtworkRoute(artworkDetail.artwork));
                    }, (error) =>
                    {
                        _model.Progress = 0.0f;
                        _model.UploadProgressActive = false;
                        _model.Active = false;
                        onResponse(new ArtworkCreateRenderer(_model));
                        _model.ApiError = error;
                        onResponse(new ApiErrorRenderer(_model));
                    });
                }, (progress) =>
                {
                    _model.Progress = progress;
                    onResponse(new ArtworkCreateRenderer(_model));
                }, (error) =>
                {
                    _model.UploadProgressActive = false;
                    onResponse(new ArtworkCreateRenderer(_model));
                    _model.ApiError = error;
                    onResponse(new ApiErrorRenderer(_model));
                });
            });
        }

        public Uri ArtworkRoute(Artwork artwork)
        {
            return new Uri(_httpHostUrl + "/artworks/" + artwork.id);
        }
    }
}
