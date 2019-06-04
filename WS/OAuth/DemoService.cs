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
using Eisk.DataAccessLayer;
using System.Configuration;

namespace OAuth2.Demo.OAuth
{
    public class DemoService : OAuthServiceBase
    {
        private int _requestTokenExpireMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["RequestTokenExpireMinutes"]);
        private int _accessTokenExpireMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["AccessTokenExpireMinutes"]);
        private MachineKeySection machineKey;
        private DateTime _defaultDate = (new DateTime(1970, 01, 01));

        static DemoService()
        {
            Tokens = new List<DemoToken>();
            RequestTokens = new Dictionary<string, DateTime>();
        }

        public static List<DemoToken> Tokens { get; set; }

        public static Dictionary<String, DateTime> RequestTokens { get; set; }

        public override OAuthResponse RequestToken(Guid deviceKey)
        {
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                Token token = new Token();

                Device device = _DatabaseContext.Devices.First(instance => instance.DeviceKey == deviceKey);
                token.DeviceID = device.DeviceID;
                token.DeviceReference.EntityKey = device.EntityKey;

                TokenType accessTokenType = _DatabaseContext.TokenTypes.First(instance => instance.TokenTypeID == 1);
                token.TokenTypeID = accessTokenType.TokenTypeID;
                token.TokenTypeReference.EntityKey = accessTokenType.EntityKey;

                token.TokenKey = Guid.NewGuid();
                token.RequestDate = DateTime.Now;
                token.ExpireDate = DateTime.Now.AddMinutes(_requestTokenExpireMinutes);

                _DatabaseContext.Tokens.AddObject(token);

                _DatabaseContext.SaveChanges();

                RequestTokens.Add(token.TokenKey.ToString("N"), token.ExpireDate);

                return new OAuthResponse
                {
                    Expires = Convert.ToInt32(new TimeSpan(token.ExpireDate.ToUniversalTime().Ticks - _defaultDate.Ticks).TotalSeconds),
                    RequestToken = token.TokenKey.ToString("N"),
                    RequireSsl = false
                };
            }
        }

        public override OAuthResponse AccessToken(Guid requestTokenKey, Guid userID)
        {
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                Token accessToken = new Token();

                Token requestToken = _DatabaseContext.Tokens.First(instance => instance.TokenKey == requestTokenKey);
                accessToken.RequestTokenKey = requestToken.TokenKey;

                Device device = _DatabaseContext.Devices.First(instance => instance.DeviceID == requestToken.DeviceID);
                accessToken.DeviceID = device.DeviceID;
                accessToken.DeviceReference.EntityKey = device.EntityKey;

                TokenType accessTokenType = _DatabaseContext.TokenTypes.First(instance => instance.TokenTypeID == 2);
                accessToken.TokenTypeID = accessTokenType.TokenTypeID;
                accessToken.TokenTypeReference.EntityKey = accessTokenType.EntityKey;

                User user = _DatabaseContext.Users.First(instance => instance.UserID == userID);
                accessToken.UserID = user.UserID;

                accessToken.TokenKey = Guid.NewGuid();
                accessToken.RequestDate = DateTime.Now;
                accessToken.ExpireDate = DateTime.Now.AddMinutes(_accessTokenExpireMinutes);

                _DatabaseContext.Tokens.AddObject(accessToken);

                _DatabaseContext.SaveChanges();

                return new OAuthResponse
                {
                    Expires = Convert.ToInt32(new TimeSpan(accessToken.ExpireDate.ToUniversalTime().Ticks - _defaultDate.Ticks).TotalSeconds),
                    AccessToken = accessToken.TokenKey.ToString("N"),
                    RequireSsl = false
                };
            }
        }

        public override OAuthResponse UpdateAccessToken(Guid accessTokenKey)
        {
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                Token newAccessToken = new Token();

                Token oldAccessToken = _DatabaseContext.Tokens.First(instance => instance.TokenKey == accessTokenKey);
                newAccessToken.RequestTokenKey = oldAccessToken.RequestTokenKey;

                Device device = _DatabaseContext.Devices.First(instance => instance.DeviceID == oldAccessToken.DeviceID);
                newAccessToken.DeviceID = device.DeviceID;
                newAccessToken.DeviceReference.EntityKey = device.EntityKey;

                TokenType accessTokenType = _DatabaseContext.TokenTypes.First(instance => instance.TokenTypeID == 2);
                newAccessToken.TokenTypeID = accessTokenType.TokenTypeID;
                newAccessToken.TokenTypeReference.EntityKey = accessTokenType.EntityKey;

                User user = _DatabaseContext.Users.First(instance => instance.UserID == oldAccessToken.UserID);
                newAccessToken.UserID = user.UserID;

                newAccessToken.TokenKey = Guid.NewGuid();
                newAccessToken.RequestDate = DateTime.Now;
                newAccessToken.ExpireDate = DateTime.Now.AddMinutes(_accessTokenExpireMinutes);

                _DatabaseContext.Tokens.AddObject(newAccessToken);

                _DatabaseContext.SaveChanges();

                return new OAuthResponse
                {
                    Expires = Convert.ToInt32(new TimeSpan(newAccessToken.ExpireDate.ToUniversalTime().Ticks - _defaultDate.Ticks).TotalSeconds),
                    AccessToken = newAccessToken.TokenKey.ToString("N"),
                    RequireSsl = false
                };
            }
        }

        public override OAuthResponse UnauthorizeToken(Guid accessTokenKey)
        {
            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                Token accessToken = _DatabaseContext.Tokens.First(instance => instance.TokenKey == accessTokenKey);

                accessToken.ExpireDate = DateTime.Now;

                _DatabaseContext.SaveChanges();

                return new OAuthResponse
                {
                    Expires = 0,
                    AccessToken = accessToken.TokenKey.ToString("N"),
                    RequireSsl = false
                };
            }
        }

        private OAuthResponse CreateAccessToken(string name)
        {
            var token = new DemoToken(name);
            Tokens.Add(token);

            return new OAuthResponse
            {
                AccessToken = token.AccessToken,
                Expires = token.ExpireSeconds,
                RequireSsl = false
            };
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
    }
}