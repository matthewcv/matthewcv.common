using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Listeners;
using Raven.Json.Linq;
using matthewcv.common.Configuration;

namespace matthewcv.common.Entity
{
    public class DocumentStoreListener : IDocumentStoreListener
    {
        private readonly ISiteConfiguration _siteConfig;

        public DocumentStoreListener(ISiteConfiguration siteConfig)
        {
            _siteConfig = siteConfig;
        }

        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
        {
            
            EntityBase ev = entityInstance as EntityBase;
            if (ev != null && original.Count == 0)
            {
                ev.CreatedDate = DateTime.UtcNow;
                ev.SiteKey = _siteConfig.SiteKey;
                return true;
            }
            return false;
        }

        public void AfterStore(string key, object entityInstance, RavenJObject metadata)
        {

        }
    }
}
