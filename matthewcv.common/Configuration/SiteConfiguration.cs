using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using matthewcv.common.Infrastructure;

namespace matthewcv.common.Configuration
{
    public interface ISiteConfiguration
    {
        /// <summary>
        /// unique key to identify a site.  Gets persisted with all entities.
        /// </summary>
        string SiteKey { get;  }

        /// <summary>
        /// name for the site.  Gets displayed in title bar, etc
        /// </summary>
        string SiteName { get;  }

        /// <summary>
        /// description of the site. 
        /// </summary>
        string SiteDescription { get;  }

        /// <summary>
        /// Whether users of the site need to be approved by admins or something.
        /// </summary>
        bool UsersMustBeApproved { get; }

        /// <summary>
        /// list of roles that the site uses.
        /// </summary>
        List<SiteRole> SiteRoles { get; }
    }

    public class SiteConfiguration:ISiteConfiguration
    {
        private List<SiteRole> _siteRoles;


        /// <summary>
        /// unique key to identify a site.  Gets persisted with all entities.
        /// </summary>
        public string SiteKey { get; set; }

        /// <summary>
        /// name for the site.  Gets displayed in title bar, etc
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// description of the site. 
        /// </summary>
        public string SiteDescription { get; set; }

        /// <summary>
        /// Whether users of the site need to be approved by admins or something.
        /// </summary>
        public bool UsersMustBeApproved { get; set; }

        /// <summary>
        /// list of roles that the site uses.
        /// </summary>
        public List<SiteRole> SiteRoles
        {
            get { return _siteRoles??(_siteRoles = new List<SiteRole>()); }

        }
    }
}
