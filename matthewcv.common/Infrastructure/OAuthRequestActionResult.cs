using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;

namespace matthewcv.common.Infrastructure
{
    public class OAuthRequestActionResult : ActionResult
    {
        private readonly OpenAuthSecurityManager _securityManager;
        private readonly string _callbackUrl;


        public OAuthRequestActionResult(OpenAuthSecurityManager securityManager, string callbackUrl)
        {
            _securityManager = securityManager;
            _callbackUrl = callbackUrl;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            _securityManager.RequestAuthentication(_callbackUrl);

        }
    }

}