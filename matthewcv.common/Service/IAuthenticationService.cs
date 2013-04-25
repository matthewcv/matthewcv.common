using System.Collections.Generic;
using OAuth2.Client;
using OAuth2.Models;
using matthewcv.common.Entity;

namespace matthewcv.common.Service
{
    public interface IAuthenticationService
    {
        IEnumerable<IClient> OAuthClients { get; }

        IClient GetClient(string providerName);

        //OpenAuthSecurityManager GetSecurityManager(string providerName);

        LoginResponse Login(UserInfo authResult);

        OAuthIdentity FindOAuthIdentity(string provider, string providerUserId);

        Profile CurrentProfile { get; }
        void UpdateCurrentProfile(Profile profile);
        void AddAuthToCurrentProfile(UserInfo verifyAuthentication);
    }
}
