using System;
using System.Runtime.Serialization;
using System.Threading;
using Pixiv.VroidSdk;
using Pixiv.VroidSdk.Api.DataModel;
using Pixiv.VroidSdk.Logger;
using Pixiv.VroidSdk.Networking.Drivers;
using UnityEngine;
using UnityEngine.UI;
using VRoidSDK.Examples.ArtworkExample.Controller;
using VRoidSDK.Examples.ArtworkExample.View;
using VRoidSDK.Examples.Core.Controller;
using VRoidSDK.Examples.Core.Localize;
using VRoidSDK.Examples.Core.Renderer;
using VRoidSDK.Examples.Core.View.Parts;
using ScreenCapture = VRoidSDK.Examples.ArtworkExample.View.ScreenCapture;

namespace VRoidSDK.Examples.ArtworkExample
{
    public class Routes : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private ArtworkRootView _rootView;
        [SerializeField] private bool _useDeviceFlow;
        [SerializeField] private ArtworkExampleEventHandler _eventHandler;
        [SerializeField] private bool _isArchived;
        [SerializeField] private ScreenCapture _capture;
#pragma warning restore 0649

        private ApiController _apiController;
        private ArtworksController _artworksController;
        private ArtworkDetailController _artworkDetailController;
        private ArtworkCreateController _artworkCreateController;

        private void Start()
        {
            var config = LoadConfigFromTextAsset();
            var driver = new HttpClientDriver(SynchronizationContext.Current);
            _apiController = new ApiController(config, driver, _useDeviceFlow);
            _artworksController = new ArtworksController(_apiController);
            _artworkDetailController = new ArtworkDetailController(_apiController);
            _artworkCreateController = new ArtworkCreateController(config.HttpHostUrl, _apiController);
        }

        private ISdkConfig LoadConfigFromTextAsset()
        {
            var asset = Resources.Load<TextAsset>("credential.json");
            if (asset == null)
            {
                throw new NullReferenceException("You have to place the credential.json.bytes in any of the Resources folders");
            }

            try
            {
                return OauthProvider.CreateSdkConfig(asset.text);
            }
            catch (SerializationException)
            {
                SdkLogger.LogError($"Could not parse textAsset: {asset.text}");
                throw;
            }
        }

        public void ShowArtworks()
        {
            _artworksController.ShowArtworks(Rendering);
        }

        public void ShowNextArtworks()
        {
            _artworksController.ShowNextArtworks(Rendering);
        }

        public void ShowArtwork(string artworkId)
        {
            _artworkDetailController.ShowArtwork(artworkId, Rendering);
        }

        public void HideArtworks()
        {
            _artworksController.HideArtworks(Rendering);
        }

        public void HideArtwork()
        {
            _artworkDetailController.HideArtwork(Rendering);
        }

        public void SendAuthorizeCode(InputField code)
        {
            var text = code.text;
            _apiController.SendAuthorizationCode(text);
        }

        public void ShowLoginPanel()
        {
            _apiController.OpenLogin(Rendering);
        }

        public void CloseLoginPanel()
        {
            _apiController.CloseLogin(Rendering);
        }

        public void Logout()
        {
            _apiController.Logout(Rendering);
            HideArtworks();
        }

        public void Login()
        {
            _apiController.Login(Rendering);
        }

        public void ShowArtworkCreateMenu()
        {
            _capture.Capture((texture) =>
            {
                _artworkCreateController.CreateArtwork(texture, _isArchived, Rendering);
            });
        }

        public void HideArtworkCreateMenu()
        {
            _artworkCreateController.HideCreateArtwork(Rendering);
        }

        public void ChangeCaption(InputField input)
        {
            _artworkCreateController.UpdateCaption(input.text);
        }

        public void ChangeAgeLimit(Toggle toggle)
        {
            var toggleValue = toggle.GetComponent<ToggleNumericValue>();
            _artworkCreateController.UpdateAgeLimit((EnumAgeLimit)toggleValue.Value, toggle.isOn);
        }

        public void UploadArtwork()
        {
            _artworkCreateController.UploadArtwork((uri) => Application.OpenURL(uri.ToString()), Rendering);
        }

        public void LocalizeChanged(int locale)
        {
            var translateLocale = (Translator.Locales)locale;
            Translator.ChangeTo(translateLocale);

            if (_eventHandler != null)
            {
                _eventHandler.OnLangChanged(translateLocale);
            }
        }

        private void Rendering(IRenderer renderer)
        {
            renderer.Rendering(_rootView);
        }
    }
}
