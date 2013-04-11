using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Raven.Client;
using matthewcv.common.Entity;

namespace matthewcv.common.Service
{
    public class AuthenticationService : IAuthenticationService, IOpenAuthDataProvider
    {
        private readonly HttpContextBase _httpContext;
        private readonly IDocumentSession _docSess;
        private List<IAuthenticationClient> _oAuthClients;

        public AuthenticationService(HttpContextBase httpContext, IDocumentSession docSess)
        {
            _httpContext = httpContext;
            _docSess = docSess;
            _oAuthClients = new List<IAuthenticationClient>();

            if (!CurrentProfile.IsGuest)
            {
                CurrentProfile.LastActivity = DateTime.UtcNow;
            }
        }

        public IList<IAuthenticationClient> OAuthClients
        {
            get { return _oAuthClients; }
        }

        public AuthenticationService AddClient(IAuthenticationClient client)
        {
            _oAuthClients.Add(client);
            return this;
        }
        
        public IAuthenticationClient GetClient(string providerName)
        {
            return OAuthClients.FirstOrDefault(c => c.ProviderName == providerName);
        }

        public OpenAuthSecurityManager GetSecurityManager(string providerName)
        {
            return new OpenAuthSecurityManager(_httpContext,GetClient(providerName),this);
        }

        public void AddAuthToCurrentProfile(AuthenticationResult verifyAuthentication)
        {
            if (CurrentProfile == null || !verifyAuthentication.IsSuccessful)
            {
                throw new Exception("Can't do that now");
            }

            //get or create the oauth identity.
            OAuthIdentity findOAuthIdentity = FindOAuthIdentity(verifyAuthentication.Provider, verifyAuthentication.ProviderUserId);
            if (findOAuthIdentity != null)
            {
                //somebody already exists. add this identity to the current profile and delete the old profile.
                Profile old = GetProfile(findOAuthIdentity.ProfileId);
                findOAuthIdentity.ProfileId = CurrentProfile.Id;
                _docSess.Delete(old); //not for sure if I really want to fully delete this profile, save it or attempt to merge data.
            }
            else
            {
                OAuthIdentity newid = new OAuthIdentity();
                newid.ProfileId = CurrentProfile.Id;
                newid.Provider = verifyAuthentication.Provider;
                newid.ProviderUserId = verifyAuthentication.ProviderUserId;
                newid.CreatedDate = DateTime.UtcNow;
                _docSess.Store(newid);
            }
        }

        public LoginResponse Login(AuthenticationResult authResult)
        {
            if (authResult.IsSuccessful == false)
            {
                throw new ApplicationException("Auth result not successful");
            }
            LoginResponse lr = new LoginResponse();
            OAuthIdentity oai = FindOAuthIdentity(authResult.Provider, authResult.ProviderUserId);
            if (oai == null)
            {
                OAuthIdentity newId = new OAuthIdentity();
                newId.Provider = authResult.Provider;
                newId.ProviderUserId = authResult.ProviderUserId;
                newId.IsCurrent = true;
                newId.LastLogin = DateTime.UtcNow;
                newId.CreatedDate = DateTime.UtcNow;

                Profile newP = new Profile();
                newP.DisplayName = authResult.UserName;
                newP.EmailAddress = authResult.UserName;
                newP.LastActivity = newP.CreatedDate = DateTime.UtcNow;
                _docSess.Store(newP);

                newId.ProfileId = newP.Id;
                _docSess.Store(newId);

                newP.OAuthIdentities.Add(newId);

                lr.Profile = newP;
                lr.NewProfileCreated = true;
            }
            else
            {
                oai.LastLogin = DateTime.UtcNow;
                oai.IsCurrent = true;
                lr.Profile = GetProfile(oai.ProfileId);
            }
            
            FormsAuthentication.SetAuthCookie(_docSess.Advanced.GetDocumentId(lr.Profile),true);

            return lr;
        }

        private Profile _currentProfile = null;
        public Profile CurrentProfile
        {
            get
            {
                if (_httpContext.User.Identity.IsAuthenticated)
                {
                    if (_currentProfile == null)
                    {
                        string id = _httpContext.User.Identity.Name;
                        string[] strings =
                            id.Split(new string[] {_docSess.Advanced.DocumentStore.Conventions.IdentityPartsSeparator},
                                     StringSplitOptions.RemoveEmptyEntries);
                        int iid;
                        if (strings.Length > 0 && int.TryParse(strings.Last(), out iid))
                        {
                            _currentProfile = GetProfile(iid);
                        }
                        else
                        {
                            throw new Exception("invalid ID format " + id);
                        }
                    }
                    return _currentProfile;
                }

                return Profile.GuestProfile();
            }
        }


        public Profile GetProfile(int id)
        {
            var profile = _docSess.Load<Profile>(id);
            if (profile != null)
            {
                profile.OAuthIdentities = _docSess.Query<OAuthIdentity>().Where(o => o.ProfileId == id).ToList();
            }
            return profile;
        }

        public OAuthIdentity FindOAuthIdentity(string provider, string providerUserId)
        {
            var oid = _docSess.Query<OAuthIdentity>().Where(i => i.Provider == provider && i.ProviderUserId == providerUserId).FirstOrDefault();
            return oid;
        }
        public OAuthIdentity FindOAuthIdentity(int id)
        {
            var oid = _docSess.Load<OAuthIdentity>(id);
            return oid;
        }

        public void UpdateCurrentProfile(Profile profile)
        {
            CurrentProfile.DisplayName = profile.DisplayName;
            CurrentProfile.EmailAddress = profile.EmailAddress;
            CurrentProfile.Location = profile.Location;
        }


        public string GetUserNameFromOpenAuth(string openAuthProvider, string openAuthId)
        {
            var oid = _docSess.Query<OAuthIdentity>().FirstOrDefault(i => i.Provider == openAuthProvider && i.ProviderUserId == openAuthId);

            if (oid == null)
            {
                return null;
            }

            var profile = _docSess.Load<Profile>(oid.ProfileId);
            if (profile == null)
            {
                return null;
            }

            return _docSess.Advanced.GetDocumentId(profile);
        }


    }
}
