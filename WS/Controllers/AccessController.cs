using Comments.Web.Filter;
using Eisk.BusinessEntities;
using OAuth2.Demo.Models;
using OAuth2.Mvc;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using WS.Authorizers;

namespace Ws.Controllers
{
    //[NoCache]
    public class AccessController : ApiController
    {
        [HttpGet]
        [AccessAuthorize("IsAuthorizedRequestToken", typeof(AccessAuthorizer))]
        public ResponseModel RequestToken()
        {
            Guid deviceKey = (Guid)HttpContext.Current.Items["deviceKey"];

            var response = OAuthServiceBase.Instance.RequestToken(deviceKey);

            return new ResponseModel(response);
        }

        [HttpGet]
        [AccessAuthorize("IsAuthorizedAccessToken", typeof(AccessAuthorizer))]
        public ResponseModel AccessToken()
        {
            var requestToken = (Token)HttpContext.Current.Items["requestToken"];
            var user = (User)HttpContext.Current.Items["user"];

            var response = OAuthServiceBase.Instance.AccessToken(requestToken.TokenKey, user.UserID);

            return new ResponseModel(response);
        }

        [HttpGet]
        [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "User")]
        public ResponseModel UpdateAccessToken()
        {
            var accessToken = (Token)HttpContext.Current.Items["accessToken"]; 

            var response = OAuthServiceBase.Instance.UpdateAccessToken(accessToken.TokenKey);

            return new ResponseModel(response);
        }

        [HttpPost]
        [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
        public ResponseModel UnauthorizeAccessToken()
        {
            var accessToken = (Token)HttpContext.Current.Items["accessToken"];
            var user = (User)HttpContext.Current.Items["user"];

            var response = OAuthServiceBase.Instance.UnauthorizeToken(accessToken.TokenKey);

            return new ResponseModel(response);
        }

    }
}
