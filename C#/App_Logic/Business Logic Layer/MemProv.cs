 
using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.ApplicationServices;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Data.Objects;
using Eisk.DataAccessLayer;
using Eisk.BusinessLogicLayer;

namespace TreeLocation.BusinessLogicLayer
{
    public partial class MemProv : MembershipProvider
    {
        #region Class Variables

        private int newPasswordLength = 8;
        private string connectionString;
        private string applicationName;
        private bool enablePasswordReset;
        private bool enablePasswordRetrieval;
        private bool requiresQuestionAndAnswer;
        private bool requiresUniqueEmail;
        private int maxInvalidPasswordAttempts;
        private int passwordAttemptWindow;
        private MembershipPasswordFormat passwordFormat;
        private int minRequiredNonAlphanumericCharacters;
        private int minRequiredPasswordLength;
        private string passwordStrengthRegularExpression;
        private MachineKeySection machineKey; //Used when determining encryption key values.

        #endregion

        #region Enums

        private enum FailureType
        {
            Password = 1,
            PasswordAnswer = 2
        }

        #endregion
        
        #region Implemantation

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            UserBLL bll = new UserBLL();
            User user = bll.GetUserByUserName(username);

            bool isValid = false;
            if (user == null || user == default(User))
            {
                return false;
            }

            if (CheckPasswordMatch(oldPassword, user.Password))
            {
                user.Password = EncodePassword(newPassword);
                isValid = true;
            }
            else
            {
                return false;
            }

            bll.UpdateUser(user);

            return isValid;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            UserBLL bll = new UserBLL();
            User user = bll.GetUserByUserName(username);

            bool isValid = false;
            if (user == null || user == default(User))
            {
                return false;
            }

            if (CheckPasswordMatch(password, user.Password))
            {
                user.PasswordQuestion = newPasswordQuestion;
                user.PasswordAnswer = newPasswordAnswer;
                isValid = true;
            }
            else
            {
                return false;
            }

            bll.UpdateUser(user);

            return isValid;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="email">Email address.</param>
        /// <param name="passwordQuestion">Security quesiton for password.</param>
        /// <param name="passwordAnswer">Security quesiton answer for password.</param>
        /// <param name="isApproved"></param>
        /// <param name="userID">User ID</param>
        /// <param name="status"></param>
        /// <returns>MembershipUser</returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            UserBLL userBLL = new UserBLL();
            ApplicationBLL applicationBLL = new ApplicationBLL();
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            MembershipUser membershipUser = GetUser(username, false);

            if (membershipUser == null)
            {
                try
                {
                    User creator = userBLL.GetUserByUserName(HttpContext.Current.User.Identity.Name);
                    User user = new User();

                    user.UserID = Guid.NewGuid();
                    user.UserName = username;
                    user.Password = EncodePassword(password);
                    user.PasswordQuestion = passwordQuestion;
                    user.PasswordAnswer = EncodePassword(passwordAnswer);
                    user.IsApproved = isApproved;
                    user.IsLockedOut = false;
                    user.IsChangePassword = false;
                    user.LastLoginDate = DateTime.Now;
                    user.LastActivityDate = DateTime.Now;
                    user.LastPasswordChangedDate = DateTime.Now;
                    user.LastLockedOutDate = DateTime.Now;
                    user.FailedPasswordAnswerAttemptCount = 0;
                    user.FailedPasswordAnswerAttemptWindowStart = DateTime.Now;
                    user.FailedPasswordAttemptCount = 0;
                    user.FailedPasswordAttemptWindowStart = DateTime.Now;
                    user.CreatedDate = DateTime.Now;
                    user.CreatorUserID = creator.UserID;
                    user.EditedDate = DateTime.Now;
                    user.EditorUserID = creator.UserID;

                    Guid ret = default(Guid);
                    ret = userBLL.CreateNewUser(user);

                    Application currentApplication = applicationBLL.GetApplicationByApplicationName(applicationName);
                    if (currentApplication == null) // If Application does not exists create it
                    {
                        currentApplication.ApplicationName = applicationName;
                        applicationBLL.CreateNewApplication(currentApplication);
                    }

                    if (ret != default(Guid))
                    {
                        status = MembershipCreateStatus.Success;
                    }
                    else
                    {
                        status = MembershipCreateStatus.UserRejected;
                    }
                }
                catch (SqlException e)
                {
                    //Add exception handling here.

                    status = MembershipCreateStatus.ProviderError;
                }

                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }


        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        
        private MembershipUser GetMembershipUserFromUser(User user)
        {
            MembershipUser membershipUser = new MembershipUser(
                this.Name,
                user.UserName,
                user.UserID,
                user.UserName,
                user.PasswordQuestion,
                user.Comment,
                (bool)user.IsApproved,
                (bool)user.IsLockedOut,
                (DateTime)user.CreatedDate,
                (DateTime)user.LastLoginDate,
                (DateTime)user.LastActivityDate,
                (DateTime)user.LastPasswordChangedDate,
                (DateTime)user.LastLockedOutDate
            );
            return membershipUser;
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
  
        public override MembershipUser GetUser(
        string username,
         bool userIsOnline
        )
        {
            MembershipUser membershipUser = null;
            UserBLL bll = new UserBLL();
            User user = bll.GetUserByUserName(username);

            if (user == null)
                return membershipUser;

            membershipUser = GetMembershipUserFromUser(user);
            if (userIsOnline)
            {
                user.LastActivityDate = DateTime.Now;

                bll.UpdateUser(user);
            }

            return membershipUser;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return passwordFormat;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

         public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return requiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate the user based upon username and password.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="password">Password.</param>
        /// <returns>T/F if the user is valid.</returns>
        public override bool ValidateUser(
         string username,
         string password
        )
        {
            UserBLL bll = new UserBLL();
            User user = bll.GetUserByUserName(username);

            bool isValid = false;
            if (user == null || user == default(User))
            {
                return false;
            }

            if (CheckPasswordMatch(password, user.Password))
            {
                user.LastLoginDate = DateTime.Now;
                user.FailedPasswordAttemptCount = 0;
                isValid = true;
            }
            else
            {
                user.FailedPasswordAttemptCount += 1;
            }

            bll.UpdateUser(user);

            return isValid;
        }

        #endregion

        #region "Utility Functions"

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
        private bool CheckPasswordMatch(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
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
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// UnEncode password.
        /// </summary>
        /// <param name="encodedPassword">Password.</param>
        /// <returns>Unencoded password.</returns>
        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }

            return configValue;
        }


        #endregion
                
        #region Initialization

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "HDIMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "How Do I: Sample Membership provider");
            }

            //Initialize the abstract base class.
            base.Initialize(name, config);

            applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredAlphaNumericCharacters"], "1"));
            minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], String.Empty));
            enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            enablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            requiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            requiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            ConnectionStringSettings ConnectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if ((ConnectionStringSettings == null) || (ConnectionStringSettings.ConnectionString.Trim() == String.Empty))
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;

            //Get encryption and decryption key information from the configuration.
            System.Configuration.Configuration cfg = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = cfg.GetSection("system.web/machineKey") as MachineKeySection;

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
            {
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                {
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");
                }
            }
        }

        #endregion
    }
}
