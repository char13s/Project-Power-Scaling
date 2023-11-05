using System;
using System.Collections.Generic;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.ArtworkExample.Renderer;
using VRoidSDK.Examples.ArtworkExample.Model;
using Pixiv.VroidSdk.Api.DataModel;

namespace VRoidSDK.Examples.ArtworkExample.Controller
{
    public class ArtworksController : BaseController
    {
        private ApiController _api;
        private ArtworkModel _model;

        public ArtworksController(ApiController api)
        {
            _api = api;
            _model = new ArtworkModel();
        }

        public void ShowArtworks(Action<IRenderer> onResponse)
        {
            CheckLogin(_api, onResponse, (account) =>
            {
                _model.CurrentUser = account;
                _model.Active = true;
                var user = account.user_detail.user;
                _api.GetUsersArtworks(user, 10, (artworks, link) =>
                {
                    _model.SetArtworks(artworks);
                    _model.Next = link;
                    onResponse(new ArtworksRenderer(_model));
                }, (error) =>
                {
                    _model.ApiError = error;
                    onResponse(new ApiErrorRenderer(_model));
                });
            });
        }

        public void ShowNextArtworks(Action<IRenderer> onResponse)
        {
            CheckLogin(_api, onResponse, (account) =>
            {
                _model.Next.next?.RequestLink<List<Artwork>>((artworks, link) =>
                {
                    _model.MergeArtworks(artworks);
                    _model.Next = link;
                    onResponse(new ArtworksRenderer(_model));
                }, (error) =>
                {
                    _model.ApiError = error;
                    onResponse(new ApiErrorRenderer(_model));
                });
            });
        }

        public void HideArtworks(Action<IRenderer> onResponse)
        {
            _model.Active = false;
            onResponse(new ArtworksRenderer(_model));
        }
    }
}
