using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using Newtonsoft.Json;
using OAuth2.Demo.Models;
using OAuth2.Mvc;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WS.Authorizers;
using WS.Interfaces;

namespace Comments.Web.Filter
{
    public class AccessAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private readonly string _method;
        private string _roles;
        private readonly Type _accessAuthorizerType;
        private string[] _rolesSplit = AuthorizationUtilities._emptyArray;

        /// <summary>
        /// The comma seperated list of roles which user needs to be in.
        /// </summary>
        public string Roles
        {

            get
            {

                return this._roles ?? string.Empty;

            }
            set
            {
                this._roles = value;
                this._rolesSplit = AuthorizationUtilities.splitString(value);
            }
        }

        public AccessAuthorizeAttribute(string method, Type accessAuthorizerType)
        {

            if (string.IsNullOrEmpty(method))
                throw new ApplicationException("apiKeyQueryParameter");

            if (accessAuthorizerType == null)
                throw new ApplicationException("apiKeyAuthorizerType");

            if (!isTypeOfIApiKeyAuthorizer(accessAuthorizerType))
            {

                throw new ApplicationException(
                    string.Format(
                        "{0} type has not implemented the TugberkUg.Web.Http.IApiKeyAuthorizer interface",
                        accessAuthorizerType.ToString()
                    )
                );
            }

            _method = method;
            _accessAuthorizerType = accessAuthorizerType;
        }

        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext, AccessAuthorizerResponse accessAuthorizerResponse)
        {
            if (actionContext == null)
            {
                throw new ApplicationException("actionContext");
            }
            var serializer = new Newtonsoft.Json.JsonSerializer();
            var respornse = new ResponseModel(accessAuthorizerResponse.AuthorizedID, accessAuthorizerResponse.Message);
            actionContext.Response = actionContext.ControllerContext.Request.CreateResponse(HttpStatusCode.Unauthorized, JsonConvert.SerializeObject(respornse));
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ApplicationException("actionContext");

            if (this.skipAuthorization(actionContext))
                return;

            AccessAuthorizerResponse accessAuthorizerResponse = authorizeCore(actionContext.Request);
            if (accessAuthorizerResponse.AuthorizedID < 1) // Not a Valid Access
                HandleUnauthorizedRequest(actionContext, accessAuthorizerResponse);
        }




        #region helpers

        //private helpers
        private bool isTypeOfIApiKeyAuthorizer(Type type)
        {

            foreach (Type interfaceType in type.GetInterfaces())
            {

                if (interfaceType == typeof(IAccessAuthorizer))
                    return true;
            }

            return false;
        }

        private bool skipAuthorization(HttpActionContext actionContext)
        {

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>() ||
                actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>();
        }
        private AccessAuthorizerResponse authorizeCore(HttpRequestMessage request)
        {
            object apiKeyAuthorizerClassInstance = Activator.CreateInstance(_accessAuthorizerType);
            AccessAuthorizerResponse result = null;

            if (_rolesSplit == AuthorizationUtilities._emptyArray)
            {
                result = (AccessAuthorizerResponse)_accessAuthorizerType.GetMethod(_method, new Type[] { typeof(HttpRequestMessage) }).
                    Invoke(apiKeyAuthorizerClassInstance, new object[] { request });
            }
            else
            {
                List<Role> roles = new List<Role>();
                foreach (var roleName in _rolesSplit)
                {
                    roles.Add(new RoleBLL().GetRoleByRoleName(roleName));
                }
                result = (AccessAuthorizerResponse)_accessAuthorizerType.GetMethod(_method, new Type[] { typeof(HttpRequestMessage), typeof(List<Role>) }).
                    Invoke(apiKeyAuthorizerClassInstance, new object[] { request, roles });
            }

            return result;
        }

        #endregion
    }

    internal class AuthorizationUtilities
    {

        public static readonly string[] _emptyArray = new string[0];

        public static string[] splitString(string original)
        {

            if (string.IsNullOrEmpty(original))
                return _emptyArray;

            IEnumerable<string> source =
                from piece in original.Split(new char[] {

					','
				})
                let trimmed = piece.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;
            return source.ToArray<string>();
        }
    }
}