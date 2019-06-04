using System;
using System.Security.Principal;
using System.Web;
using OAuth2.Mvc;

namespace OAuth2.Demo.OAuth
{
    public class DemoProvider : OAuthProviderBase
    {
        public override IIdentity RetrieveIdentity(HttpContext context)
        {
            var token = context.Request.GetToken();
            return String.IsNullOrEmpty(token)
                ? null
                : new DemoIdentity(this, token);
        }

        public override IPrincipal CreatePrincipal(IIdentity identity)
        {
            return identity == null || !(identity is DemoIdentity)
                ? null
                : new DemoPrincipal(this, identity);
        }
    }
}