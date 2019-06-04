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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;
using System.Web;

namespace Eisk.BusinessLogicLayer
{
    public partial class UserInfoBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<UserInfo> GetUserInfoByFilter(string UserName = default(string  ), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1)
        {

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "UserID";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<UserInfo> UserInfos = (
                    from UserInfoList in
                        _DatabaseContext.UserInfoes
                        .DynamicOrderBy(orderBy)
                    where
                       (UserInfoList.User.UserName == UserName )
                    select UserInfoList
                )
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList();

            return UserInfos;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public UserInfo GetCurrentUserUserInfo(string UserName)
        {
           return (_DatabaseContext.UserInfoes.FirstOrDefault(UserInfo => UserInfo.User.UserName == UserName));
        }
    }
}
