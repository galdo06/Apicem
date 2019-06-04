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
    public partial class GroupBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Group GetGroupByGroupId2(int groupID)
        {
            //Validate Input
            if (groupID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("groupID");
            Group group = (_DatabaseContext.Groups.FirstOrDefault(instance => instance.GroupID == groupID));

            // var ass = group.GroupCreator.UserName;
            return group;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group> GetGroupByFilter(string groupName = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "groupName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var groups = (
                    from groupList in
                        _DatabaseContext.Groups
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(groupName) || groupList.GroupName.ToUpper().Trim().Contains(groupName.ToUpper().Trim()))
                    select groupList
                );


            var groupsList = groups.Skip(startRowIndex).Take(maximumRows).ToList();

            HttpContext.Current.Items["groups_Count"] = groups.Count();

            return groupsList;
        }

        public int GetTotalCountForAllGroups(string groupName = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            var count = (int)HttpContext.Current.Items["groups_Count"];
            HttpContext.Current.Items.Remove("groups_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "groupName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<Group> groups = (
                    from groupList in
                        _DatabaseContext.Groups
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(groupName) || groupList.GroupName.ToUpper().Contains(groupName.ToUpper()))
                    select groupList
                )
                .ToList();
            return groups.Count();
             */
        }

        public List<Group> GetAllGroupsList()
        {
            List<Group> roles = _DatabaseContext.Groups.ToList();

            return roles;
        }

        public void AddUserToGroup(User user, Group group)
        {
            Group_UsersBLL group_UsersBLL = new Group_UsersBLL();

            Group_Users group_User = new BusinessEntities.Group_Users();

            group_User.GroupID = group.GroupID;
            group_User.GroupReference.EntityKey = group.EntityKey;

            group_User.UserID = user.UserID;
            group_User.UserReference.EntityKey = user.EntityKey;

            group_UsersBLL.CreateNewGroup_Users(group_User);
        }

    }
}
