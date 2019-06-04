using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using OAuth2.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using WS.Interfaces;

namespace WS.Authorizers
{
    public class AccessAuthorizerResponse
    {
        public int AuthorizedID { get; set; }
        public string Message { get; set; }

        public AccessAuthorizerResponse(int _ErrorID, string _Message)
        {
            AuthorizedID = _ErrorID;
            Message = _Message;
        }
    }

    public class AccessAuthorizer : IAccessAuthorizer
    {
        private int _accessTokenExpireMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["AccessTokenExpireMinutes"]);
        private MachineKeySection machineKey;

        // 1) RequestToken = with Device ID return a RequestToken
        public AccessAuthorizerResponse IsAuthorizedRequestToken(HttpRequestMessage request)
        {
            Guid deviceKey;
            // Find Header
            if (request.Headers.Contains(OAuthConstants.AuthorzationHeader))
            {

                IEnumerable<string> authorzationHeaders = request.Headers.GetValues(OAuthConstants.AuthorzationHeader);
                if (authorzationHeaders.Count() == 1)
                {
                    string deviceKeyString = authorzationHeaders.FirstOrDefault();

                    if (!Guid.TryParse(deviceKeyString, out deviceKey))
                        return new AccessAuthorizerResponse(0, "DeviceKey inválido");

                    if (!(new DeviceBLL().DeviceKeyExists(deviceKey)))
                    {
                        return new AccessAuthorizerResponse(0, "DeviceKey inválido");
                    }

                    Device device = new DeviceBLL().GetDeviceByDeviceKey(deviceKey);


                    if (!device.Active)
                    {
                        return new AccessAuthorizerResponse(-1, "DeviceKey inactivo. Favor de contactar al desarrollador.");
                    }

                    HttpContext.Current.Items["deviceKey"] = deviceKey;
                    return new AccessAuthorizerResponse(1, "DeviceKey válido"); ;
                }
            }

            return new AccessAuthorizerResponse(0, "DeviceKey inválido");
        }

        // 2) AccessToken  = with RequestToken and UserName:Password return AccessToken
        public AccessAuthorizerResponse IsAuthorizedAccessToken(HttpRequestMessage request)
        {
            UserBLL bll = new UserBLL();
            User user = new User();
            Token requestToken = new Token();

            if (request.Headers.Contains(OAuthConstants.Credentials))
            {
                IEnumerable<string> credentialsHeaders = request.Headers.GetValues(OAuthConstants.Credentials);
                if (credentialsHeaders.Count() == 1)
                {
                    var credentialsHeader = credentialsHeaders.FirstOrDefault();
                    string[] usernamePassword = credentialsHeader.Trim().Split(':').Where(instance => !string.IsNullOrEmpty(instance)).ToArray();
                    if (usernamePassword.Length < 2)
                        return new AccessAuthorizerResponse(0, "Email o Contraseña inválida");

                    string username = usernamePassword[0];
                    string password = usernamePassword[1];

                    user = bll.GetUserByUserName(username);

                    if (user == null || user == default(Eisk.BusinessEntities.User))
                        return new AccessAuthorizerResponse(0, "Email o Contraseña inválida");

                    if (!CheckPassword(password, user.Password))
                    {
                        user.FailedPasswordAttemptCount += 1;
                        bll.UpdateUser(user);
                        return new AccessAuthorizerResponse(0, "Email o Contraseña inválida");
                    }
                }
            }
            else
            {
                return new AccessAuthorizerResponse(0, "Email o Contraseña inválida");
            }

            if (request.Headers.Contains(OAuthConstants.AuthorzationHeader))
            {
                IEnumerable<string> authorzationHeaders = request.Headers.GetValues(OAuthConstants.AuthorzationHeader);
                if (authorzationHeaders.Count() == 1)
                {
                    string stringRequestToken = authorzationHeaders.FirstOrDefault();

                    Guid guidRequestTokenKey;
                    if (!Guid.TryParse(stringRequestToken, out guidRequestTokenKey))
                    {
                        user.FailedPasswordAttemptCount += 1;
                        bll.UpdateUser(user);
                        return new AccessAuthorizerResponse(-1, "Token inválido");
                    }

                    requestToken = new TokenBLL().GetTokenByTokenKey(guidRequestTokenKey);

                    if (requestToken == null || requestToken.ExpireDate < DateTime.Now || requestToken.RequestTokenKey != null)
                    {
                        return new AccessAuthorizerResponse(-1, "Token inválido");
                    }
                }
                else
                {
                    return new AccessAuthorizerResponse(-1, "Token inválido");
                }
            }
            else
            {
                return new AccessAuthorizerResponse(-1, "Token inválido");
            }


            user.LastLoginDate = DateTime.Now;
            user.FailedPasswordAttemptCount = 0;
            bll.UpdateUser(user);

            HttpContext.Current.Items["requestToken"] = requestToken;
            HttpContext.Current.Items["user"] = user;

            return new AccessAuthorizerResponse(1, "Token válido"); ;
        }

        // 3) RetreiveData = with AccessToken retreive date (IMPORTANT: Need Authorization)
        public AccessAuthorizerResponse IsAuthorizedRetreiveData(HttpRequestMessage request, List<Role> roles)
        {
            UserBLL userBLL = new UserBLL();
            TokenBLL tokenBLL = new TokenBLL();
            User user = new User();
            Token accessToken = new Token();

            if (request.Headers.Contains(OAuthConstants.AuthorzationHeader))
            {
                IEnumerable<string> authorzationHeaders = request.Headers.GetValues(OAuthConstants.AuthorzationHeader);
                if (authorzationHeaders.Count() == 1)
                {
                    string stringAccessToken = authorzationHeaders.FirstOrDefault();

                    Guid guidAccessTokenKey;
                    if (!Guid.TryParse(stringAccessToken, out guidAccessTokenKey))
                        return new AccessAuthorizerResponse(-1, "Token inválido");

                    accessToken = tokenBLL.GetTokenByTokenKey(guidAccessTokenKey);

                    if (accessToken == null || accessToken == new Token())
                        return new AccessAuthorizerResponse(-1, "Token inválido");

                    if (accessToken.ExpireDate < DateTime.Now || accessToken.RequestTokenKey == null)
                        return new AccessAuthorizerResponse(-1, "Token inválido");

                    user = userBLL.GetUserByUserID((Guid)accessToken.UserID);

                    List<Role> user_Roles = new Role_UsersBLL().GetRole_UsersByUserID(user.UserID).Select(instance => instance.Role).ToList();

                    List<Role> matchingRoles = (
                                                    from userRls in user_Roles
                                                    join rls in roles
                                                    on userRls.RoleID equals rls.RoleID
                                                    select userRls
                                                )
                                                .ToList();

                    if (matchingRoles.Count == 0)
                        return new AccessAuthorizerResponse(-1, "Token inválido");

                    accessToken.ExpireDate = DateTime.Now.AddMinutes(_accessTokenExpireMinutes);
                    tokenBLL.UpdateToken(accessToken);

                    HttpContext.Current.Items["accessToken"] = accessToken;
                    HttpContext.Current.Items["user"] = user;

                    return new AccessAuthorizerResponse(1, "Token válido");
                }
            }

            return new AccessAuthorizerResponse(-1, "Token inválido");
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