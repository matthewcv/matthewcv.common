using System;

namespace matthewcv.common.Entity
{
    public class OAuthIdentity : EntityBase
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public int ProfileId { get; set; }

        /// <summary>
        /// last time this identity was used.
        /// </summary>
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// this is a flag that says this is the identity that the user used to log in to the current session.
        /// </summary>
        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public bool IsCurrent { get; set; }


    }
}
