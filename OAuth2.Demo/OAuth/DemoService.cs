using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using OAuth2.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Data.Entity;

namespace OAuth2.Demo.OAuth
{
    public class DemoService : OAuthServiceBase
    {
        private MachineKeySection machineKey;

        static DemoService()
        {
            Tokens = new List<DemoToken>();
            RequestTokens = new Dictionary<string, DateTime>();
        }

        public static List<DemoToken> Tokens { get; set; }

        public static Dictionary<String, DateTime> RequestTokens { get; set; }

        public override OAuthResponse RequestToken()
        {
            var token = Guid.NewGuid().ToString("N");
            var expire = DateTime.Now.AddMinutes(5);
            RequestTokens.Add(token, expire);

            return new OAuthResponse
                   {
                       Expires = (int)expire.Subtract(DateTime.Now).TotalSeconds,
                       RequestToken = token,
                       RequireSsl = false,
                       Success = true
                   };
        }

        public override OAuthResponse AccessToken(string requestToken, string grantType, string userName, string password, bool persistent)
        {
            OAuthResponse response = new OAuthResponse();

            UserBLL bll = new UserBLL();
            User user = bll.GetUserByUserName(userName);

           if (user != null && user != default(Eisk.BusinessEntities.User) && CheckPassword(password, user.Password))
            {
                user.LastLoginDate = DateTime.Now;
                user.FailedPasswordAttemptCount = 0;

                response = CreateAccessToken(userName);
            }
            else
            {
                user.FailedPasswordAttemptCount += 1;
                response =  new OAuthResponse { Success = false };
            }

            bll.UpdateUser(user);

            return response;
        }

        public override OAuthResponse RefreshToken(string refreshToken)
        {
            var token = Tokens.FirstOrDefault(t => t.RefreshToken == refreshToken);

            if (token == null)
                return new OAuthResponse
                       {
                           Error = "RefreshToken not found.",
                           Success = false
                       };

            if (token.IsRefreshExpired)
                return new OAuthResponse
                       {
                           Error = "RefreshToken expired.",
                           Success = false
                       };

            Tokens.Remove(token);
            return CreateAccessToken(token.Name);
        }

        private OAuthResponse CreateAccessToken(string name)
        {
            var token = new DemoToken(name);
            Tokens.Add(token);

            return new OAuthResponse
            {
                AccessToken = token.AccessToken,
                Expires = token.ExpireSeconds,
                RefreshToken = token.RefreshToken,
                RequireSsl = false,
                Success = true
            };
        }

        public override bool UnauthorizeToken(string accessToken)
        {
            var token = Tokens.FirstOrDefault(t => t.AccessToken == accessToken);
            if (token == null)
                return false;

            Tokens.Remove(token);
            return true;
        }

        #region Authentication

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// Check the password format based upon the MembershipPasswordFormat.
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="dbpassword"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool CheckPassword(string password, string dbpassword)
        {
            if (EncodePassword(password) == dbpassword)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Encode password.
        /// </summary>
        /// <param name="password">Password.</param>
        /// <returns>Encoded password.</returns>
        private string EncodePassword(string password)
        {           
            //Get encryption and decryption key information from the configuration.
            System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = cfg.GetSection("system.web/machineKey") as MachineKeySection;

            string encodedPassword = password;

            HMACSHA1 hash = new HMACSHA1();
            hash.Key = HexToByte(machineKey.ValidationKey);
            encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

            return encodedPassword;
        }

        #endregion

        public override OAuthResponse RequestToken(Guid deviceID)
        {
            throw new NotImplementedException();
        }

        public override OAuthResponse AccessToken(Guid requestTokenKey, Guid userID)
        {
            throw new NotImplementedException();
        }

        public override OAuthResponse UpdateAccessToken(Guid accessTokenKey)
        {
            throw new NotImplementedException();
        }

        public override OAuthResponse UnauthorizeToken(Guid accessTokenKey)
        {
            throw new NotImplementedException();
        }
    }
}