using System;
using System.Collections.Generic;

namespace matthewcv.common.Entity
{
    public class Profile : EntityBase
    {
        private IList<OAuthIdentity> _oAuthIdentities;
        private List<string> _roles;

        /// <summary>
        /// the profile's display name for the site
        /// </summary>

        public string DisplayName { get; set; }

        public string EmailAddress { get; set; }

        public string Location { get; set; }

        public string PersonalDescription { get; set; }

        public DateTime LastActivity { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public List<String> Roles
        {
            get { return _roles ?? (_roles = new List<string>()); }
            set { _roles = value; }
        }

        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public bool IsGuest { get; set; }

        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public IList<OAuthIdentity> OAuthIdentities 
        {
            get { return _oAuthIdentities ?? (_oAuthIdentities = new List<OAuthIdentity>()); }
            set { _oAuthIdentities = value; }
        }

        internal static Profile GuestProfile()
        {
            return new Profile() {IsGuest = true, DisplayName = "Friend"};
        }
    }
}
