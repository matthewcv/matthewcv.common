using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Listeners;
using Raven.Json.Linq;
using matthewcv.common.Entity;

namespace matthewcv.common.Configuration
{
    public class ConfigurationModule:Ninject.Modules.NinjectModule
    {
        private static SiteConfiguration _configuration = new SiteConfiguration();
        private string _ravenConnectionStringName;
        public ConfigurationModule()
        {
            WithSiteRole("Admin");  //every site needs an Admin role, so just put it in there automatically.
            WithUsersMustBeApproved(true); //do this by default.  
        }
        public override void Load()
        {
            Bind<IDocumentStoreListener>().To<DocumentStoreListener>().InSingletonScope();
            Bind<ISiteConfiguration>().ToMethod(c => _configuration);
            Bind<IDocumentStore>()
                .ToMethod(InitDocStore)
                .InSingletonScope();

            

            Bind<IDocumentSession>()
                .ToMethod(c => c.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation((c, d) =>
                {
                    
                    if (d.Advanced.HasChanges)
                    {
                        d.SaveChanges();
                    }
                });


        }
        

        private IDocumentStore InitDocStore(IContext context)
        {
            
            DocumentStore ds = new DocumentStore { ConnectionStringName = _ravenConnectionStringName };
            ds.Initialize();
            ds.RegisterListener(context.Kernel.Get<IDocumentStoreListener>());
            return ds;
        }

        public ConfigurationModule WithRavenConnectionStringName(string conName)
        {
            _ravenConnectionStringName = conName;
            return this;
        }

        public ConfigurationModule WithSiteKey(string key)
        {
            _configuration.SiteKey = key;
            return this;
        }
        public ConfigurationModule WithSiteName(string name)
        {
            _configuration.SiteName = name;
            return this;
        }
        public ConfigurationModule WithSiteDescription(string desc)
        {
            _configuration.SiteDescription = desc;
            return this;
        }
        public ConfigurationModule WithSiteRole(string name, string desc = null)
        {
            if (desc == null) desc = name;
            _configuration.SiteRoles.Add(new SiteRole(){Name=name,Description = desc});
            return this;
        }
        public ConfigurationModule WithUsersMustBeApproved(bool val)
        {
            _configuration.UsersMustBeApproved = val;
            return this;
        }
}
}
