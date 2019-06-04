/****************** Copyright Notice *****************
 
This code is licensed under Microsoft Public License (Ms-PL). 
You are free to use, modify and distribute any portion of this code. 
The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:

=======================================================
   
Architecture Designed and Implemented By:
Mohammad Ashraful Alam
Microsoft Most Valuable Professional, ASP.NET 2007 – 2011
Twitter: http://twitter.com/AshrafulAlam | Blog: http://blog.ashraful.net | Portfolio: http://www.ashraful.net
   
*******************************************************/


using System;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Data.Objects;
using Eisk.DataAccessLayer;

namespace Eisk.BusinessLogicLayer
{
    public partial class UserBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<User> GetUsersByFilter(string userName = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "userName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var users = (
                    from usersList in
                        _DatabaseContext.Users
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(userName) || usersList.UserName.ToUpper().Trim().Contains(userName.ToUpper().Trim()))
                    select usersList
                );

            var returList = users.Skip(startRowIndex).Take(maximumRows).ToList();

            HttpContext.Current.Items["users_Count"] = users.Count();

            return returList;
        }

        public int GetTotalCountForAllUsers(string userName = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            var count = (int)HttpContext.Current.Items["users_Count"];
            HttpContext.Current.Items.Remove("users_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "userName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<User> userNames = (
                    from userNameList in
                        _DatabaseContext.Users
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(userName) || userNameList.UserName.ToUpper().Contains(userName.ToUpper()))
                    select userNameList
                )
                .ToList();
            return userNames.Count();
            */
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public int GetNumberOfUsersOnline()
        {
            List<User> users = (
                    from usersList in
                        _DatabaseContext.Users
                    where
                       (usersList.IsOnLine == true)
                    select usersList
                )
                .ToList();

            return users.Count;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public User GetUserByUserName(string userName)
        {
            var user = _DatabaseContext.Users.FirstOrDefault(instance => instance.UserName == userName);

            return user;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public User GetUserByUserName_WithoutApplication(string userName)
        {
            if (_DatabaseContext.Users.Count() > 0)
            {
                return _DatabaseContext.Users.FirstOrDefault(user => user.UserName == userName);
            }
            else
                return null;
        }
    }
}
