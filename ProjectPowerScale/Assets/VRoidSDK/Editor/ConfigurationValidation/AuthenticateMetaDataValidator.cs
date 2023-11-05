using System.Reflection;
using UnityEngine;
using Pixiv.VroidSdk.Legacy;

namespace Pixiv.VroidSdk.Editor
{
    public class AuthenticateMetaDataValidator : IValidator
    {
#pragma warning disable 0618
        private SDKConfiguration _configuration;
#pragma warning restore 0618

#pragma warning disable 0618
        public AuthenticateMetaDataValidator(SDKConfiguration configuration)
#pragma warning restore 0618
        {
            _configuration = configuration;
        }

        public bool Validate()
        {
            var appIdField = _configuration.GetType().GetField("_applicationId", BindingFlags.Instance | BindingFlags.NonPublic);
            var secretIdField = _configuration.GetType().GetField("_secret", BindingFlags.Instance | BindingFlags.NonPublic);

            var appId = (string)appIdField.GetValue(_configuration);
            var secret = (string)secretIdField.GetValue(_configuration);

            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(secret))
            {
                return false;
            }

            return true;
        }
    }
}
