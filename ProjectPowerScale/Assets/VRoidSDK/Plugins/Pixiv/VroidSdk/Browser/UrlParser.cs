using System;

namespace Pixiv.VroidSdk.Browser
{
    internal class AuthCodeUrlQuery
    {
        public string AuthCode { get; set; }
        public string State { get; set; }
    }

    internal class InvalidCallbackUrlException : Exception
    {
        public InvalidCallbackUrlException(string message) : base(message) { }
    }

    internal static class UrlParser
    {
        public static AuthCodeUrlQuery ParseAuthCode(string url, string redirectUri)
        {
            if (!IsOAuthCallbackUrl(url, redirectUri))
            {
                throw new InvalidCallbackUrlException("url is empty or invalid redirectUrl");
            }

            string authCode = "";
            string state = "";
            string[] pathQuery = url.Split('?');
            if (pathQuery.Length > 1)
            {
                string[] urlQueryPairs = pathQuery[pathQuery.Length - 1].Split('&');
                for (int i = 0; i < urlQueryPairs.Length; ++i)
                {
                    string[] keyValue = urlQueryPairs[i].Split('=');
                    if (keyValue.Length > 1)
                    {
                        switch (keyValue[0])
                        {
                            case "code":
                                authCode = keyValue[1];
                                break;
                            case "state":
                                state = keyValue[1];
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return new AuthCodeUrlQuery
            {
                State = state,
                AuthCode = authCode
            };
        }

        private static bool IsOAuthCallbackUrl(string url, string redirectUri)
        {
            var urlStrings = url.Split('?');
            if (urlStrings.Length <= 0)
            {
                return false;
            }

            var redirect = new Uri(redirectUri);
            var requestUri = new Uri(urlStrings[0]);

            return redirect == requestUri;
        }
    }
}
