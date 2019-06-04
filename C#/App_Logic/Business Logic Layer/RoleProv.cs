 
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
using System.Data;
using System.Collections;
using Eisk.BusinessLogicLayer;

namespace TreeLocation.BusinessLogicLayer
{
    public partial class RoleProv : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string username in usernames)
            {
                foreach (string roleName in roleNames)
                {
                    UserBLL userBLL = new UserBLL();
                    RoleBLL roleBLL = new RoleBLL();

                    Role role = roleBLL.GetRoleByRoleName(roleName);
                    User user = userBLL.GetUserByUserName(username);

                    roleBLL.AddUserToRole(user, role);
                }
            }
        }

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

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            RoleBLL roleBLL = new RoleBLL();
            return roleBLL.GetAllRolesString();
        }

        public override string[] GetRolesForUser(string username)
        {
            UserBLL userBLL = new UserBLL();
            User user = userBLL.GetUserByUserName(username);

            return user.Role_Users.Select(instance => instance.Role.RoleName).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
