using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using matthewcv.common.Entity;
using matthewcv.common.Service;

namespace matthewcv.common.Infrastructure
{

    public abstract class PageBase<T> : System.Web.Mvc.WebViewPage<T>
    {
        [Inject]
        public IAuthenticationService AuthService { get; set; }
        public Profile CurrentProfile 
        { 
            get
            {
                return AuthService.CurrentProfile;   
            }
        }


    }
}