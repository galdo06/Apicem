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
using System.Data;
using System.Collections;

namespace Eisk.BusinessLogicLayer
{
    public partial class RoleBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public Role GetRoleByRoleName(string roleName)
        {
            //Validate Input
            if (roleName.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("roleName");
            return (_DatabaseContext.Roles.FirstOrDefault(role => role.RoleName == roleName));
        }

        public List<Role> GetAllRolesList()
        {
            List<Role> roles = _DatabaseContext.Roles.ToList();

            return roles;
        }


        public void AddUserToRole(User user, Role role)
        {
            Role_UsersBLL role_UsersBLL = new Role_UsersBLL();

            Role_Users role_User = new BusinessEntities.Role_Users();

            role_User.RoleID = role.RoleID;
            role_User.RoleReference.EntityKey = role.EntityKey;

            role_User.UserID = user.UserID;
            role_User.UserReference.EntityKey = user.EntityKey;

            role_UsersBLL.CreateNewRole_Users(role_User);
        }

        public string[] GetAllRolesString()
        {
            return _DatabaseContext.Roles.Select(instance => instance.RoleName).ToArray();
        }
    }
}
