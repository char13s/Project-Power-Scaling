using System.Collections.Generic;
using System;
using Pixiv.VroidSdk.Legacy;

namespace Pixiv.VroidSdk.Editor
{
    public class IOSUriSchemeValidator : IValidator
    {
        private UriSchemeValidator _validator;

#pragma warning disable 0618
        public IOSUriSchemeValidator(SDKConfiguration configuration)
#pragma warning restore 0618
        {
            _validator = new UriSchemeValidator(configuration.IOSUrlScheme);
        }

        public bool Validate()
        {
            return _validator.Validate();
        }
    }
}
