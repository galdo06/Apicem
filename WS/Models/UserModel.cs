using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using OAuth2.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web;

namespace WS.Models
{
    public class UserModel
    {
        public static User GetUserFromAuthorizationHeader(HttpRequest request)
        {
            string accessTokenKey = request.Headers.GetValues(OAuthConstants.AuthorzationHeader).FirstOrDefault();

            Guid userID = (Guid)new TokenBLL().GetTokenByTokenKey(Guid.Parse(accessTokenKey)).UserID;

            return GetUser(userID);
        }

        public static User GetUser(HttpRequestMessage request)
        {
            string userIDString = request.Headers.GetValues(OAuthConstants.UserID).FirstOrDefault();

            return GetUser(userIDString);
        }

        public static User GetUser(string userIDString)
        {
            return GetUser(Guid.Parse(userIDString));
        }

        public static User GetUser(Guid userID)
        {
            return new UserBLL().GetUserByUserID(userID);
        }

        public static List<Group> GetUserGroups(HttpRequestMessage request)
        {
            User user = GetUser(request);

            return GetUserGroups(user);
        }

        public static List<Group> GetUserGroups(User user)
        {
            return user.Group_Users.Select(instance => instance.Group).ToList();
        }

    }
}