using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using matthewcv.common.Entity;

namespace matthewcv.common.Service
{
    public interface IAuthenticationService
    {
        IList<IAuthenticationClient> OAuthClients { get; }

        IAuthenticationClient GetClient(string providerName);

        OpenAuthSecurityManager GetSecurityManager(string providerName);

        LoginResponse Login(AuthenticationResult authResult);

        OAuthIdentity FindOAuthIdentity(string provider, string providerUserId);

        Profile CurrentProfile { get; }
        void UpdateCurrentProfile(Profile profile);
        void AddAuthToCurrentProfile(AuthenticationResult verifyAuthentication);
    }
}
