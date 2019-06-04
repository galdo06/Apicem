using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OAuth2.Demo.Attributes;
using OAuth2.Demo.Models;
using OAuth2.Mvc;

namespace OAuth2.Demo.Controllers
{
    [NoCache]
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index(string returnUrl)
        {
            var response = OAuthServiceBase.Instance.RequestToken();

            return View(new LoginModel
            {
                RequestToken = response.RequestToken,
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public ActionResult Index(string requestToken, string username, string password, bool? rememberMe, string returnUrl)
        {
            var accessResponse = OAuthServiceBase.Instance.AccessToken(requestToken, "User", username, password, rememberMe.HasValue && rememberMe.Value);

            if (!accessResponse.Success)
            {
                OAuthServiceBase.Instance.UnauthorizeToken(requestToken);
                var requestResponse = OAuthServiceBase.Instance.RequestToken();

                return View(new LoginModel
                {
                    RequestToken = requestResponse.RequestToken,
                    Username = username,
                    RememberMe = rememberMe.HasValue && rememberMe.Value,
                    ErrorMessage = "Invalid Credentials",
                    ReturnUrl = returnUrl
                });
            }

            ViewData["ReturnUrl"] = String.IsNullOrEmpty(returnUrl)
                                          ? "/"
                                          : returnUrl;

            return View("Success", accessResponse);
        }
    }
}
