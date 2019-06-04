using OAuth2.Mvc;
using System;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Comments.Web.Filter
{
    public class AuthorizeTokenAttribute : AuthorizationFilterAttribute
    {
        private const string TOKEN_HEADER = "Authorization";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                if (!AuthorizeRequest(actionContext.ControllerContext.Request))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { RequestMessage = actionContext.ControllerContext.Request };
                }
                return;
            }
        }

        private bool AuthorizeRequest(HttpRequestMessage request)
        {
            bool authorized = false;
            
            bool validDeviceID = false;
            bool validRequestToken = false;
            bool ValidCredentials = false;
            bool validAuthorizationToken = false;

            // Find Header
            if (request.Headers.Contains(OAuthConstants.AuthorzationHeader))
            {
                var tokenValue = request.Headers.GetValues(OAuthConstants.AuthorzationHeader); 
                if (tokenValue.Count() == 1)
                {
                    var value = tokenValue.FirstOrDefault();
                    var header = new AuthorizationHeader(value);

                    switch (header.Scheme)
                    {
                        case "Request":
                            //validDeviceID = ValidateDeviceID(header.ParameterText.Trim());
                            break;
                    }

                    if (string.Equals(header.Scheme, "OAuth", StringComparison.OrdinalIgnoreCase))


                    if (string.Equals(header.Scheme, "OAuth", StringComparison.OrdinalIgnoreCase))
                        return true;// header.ParameterText.Trim();


                    //Token validation logic here
                    //set authorized variable accordingly
                }
            }
            return authorized;
        }
    }
}