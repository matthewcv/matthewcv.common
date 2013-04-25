using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using OAuth2.Client;
using OAuth2.Configuration;
using OAuth2.Models;
using Raven.Client;
using matthewcv.common.Entity;
using matthewcv.common.Infrastructure;

namespace matthewcv.common.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;
        private readonly IDocumentSession _docSess;
        private readonly IEnumerable<IClient> _clients;

        private readonly OAuth2ConfigurationSection _configSection;

        public AuthenticationService(HttpContextBase httpContext, IDocumentSession docSess, IEnumerable<IClient> clients )
        {
            _httpContext = httpContext;
            _docSess = docSess;
            _configSection = (OAuth2ConfigurationSection)System.Configuration.ConfigurationManager.GetSection("oauth2") ;
            IEnumerable<string> enabled = _configSection.Services.AsEnumerable().Where(c => c.IsEnabled).Select(c => c.ProviderName);
            _clients = clients.Where(c => enabled.Contains(c.ProviderName));


            if (!CurrentProfile.IsGuest)
            {
                CurrentProfile.LastActivity = DateTime.UtcNow;
            }
        }

        public IEnumerable<IClient> OAuthClients
        {
            get { return _clients; }
        }


        public IClient GetClient(string providerName)
        {
            return OAuthClients.FirstOrDefault(c => c.ProviderName == providerName);
        }


        public void AddAuthToCurrentProfile(UserInfo userinfo)
        {
            if (CurrentProfile == null || userinfo == null)
            {
                throw new Exception("Can't do that now");
            }

            //get or create the oauth identity.
            OAuthIdentity findOAuthIdentity = FindOAuthIdentity(userinfo.ProviderName, userinfo.Id);
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
                newid.Provider = userinfo.ProviderName;
                newid.ProviderUserId = userinfo.Id;
                _docSess.Store(newid);
            }
        }

        public LoginResponse Login(UserInfo userinfo)
        {
            if (userinfo == null)
            {
                throw new ApplicationException("Auth result not successful");
            }
            LoginResponse lr = new LoginResponse();
            OAuthIdentity oai = FindOAuthIdentity(userinfo.ProviderName, userinfo.Id);
            if (oai == null)
            {
                OAuthIdentity newId = new OAuthIdentity();
                newId.Provider =userinfo.ProviderName;
                newId.ProviderUserId = userinfo.Id;
                newId.IsCurrent = true;
                newId.LastLogin = DateTime.UtcNow;

                Profile newP = new Profile();
                newP.DisplayName = userinfo.FirstName + " " + userinfo.LastName;
                newP.EmailAddress = userinfo.Email;
                newP.LastActivity = DateTime.UtcNow;
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
