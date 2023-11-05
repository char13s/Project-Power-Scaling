using System.Collections.Generic;
using System;
using Pixiv.VroidSdk.Legacy;

namespace Pixiv.VroidSdk.Editor
{
    public class AndroidUriSchemeValidator : IValidator
    {
        private UriSchemeValidator _validator;

#pragma warning disable 0618
        public AndroidUriSchemeValidator(SDKConfiguration configuration)
#pragma warning restore 0618
        {
            _validator = new UriSchemeValidator(configuration.AndroidUrlScheme);
        }

        public bool Validate()
        {
            return _validator.Validate();
        }
    }
}
