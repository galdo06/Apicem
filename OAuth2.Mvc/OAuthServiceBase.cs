using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using WS.Models;

namespace OAuth2.Mvc
{
    public interface IOAuthService
    {
        OAuthResponse RequestToken(Guid deviceID);
        OAuthResponse AccessToken(Guid requestTokenKey, Guid userID);
        OAuthResponse UpdateAccessToken(Guid accessTokenKey);
        OAuthResponse UnauthorizeToken(Guid accessTokenKey);

        void Initialize(string name, NameValueCollection config);
        string Name { get; }
        string Description { get; }
    }

    public abstract class OAuthServiceBase : ProviderBase, IOAuthService
    {
        public static IOAuthService Instance { get; set; }

        public abstract OAuthResponse RequestToken(Guid deviceID);

        public abstract OAuthResponse AccessToken(Guid requestTokenKey, Guid userID);

        public abstract OAuthResponse UpdateAccessToken(Guid accessTokenKey);
        
        public abstract OAuthResponse UnauthorizeToken(Guid accessTokenKey);
    }
}