//------------------------------------------------------------------------------
// <auto-generated>
//
// ***************** Copyright Notice *****************
// 
// This code is licensed under Microsoft Public License (Ms-PL). 
// You are free to use, modify and distribute any portion of this code. 
// The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:
//
// =======================================================
//   
// Architecture Designed and Implemented By:
// Mohammad Ashraful Alam
// Microsoft Most Valuable Professional, ASP.NET 2007 – 2011
// Twitter: http://twitter.com/AshrafulAlam | Blog: http://blog.ashraful.net | Portfolio: http://www.ashraful.net
//   
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.DataAccessLayer;
using Eisk.Helpers;

namespace Eisk.BusinessLogicLayer
{   
	[System.ComponentModel.DataObject(true)]
	public partial class Group_UsersBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public Group_UsersBLL() : this(new DatabaseContext()) { }

        public Group_UsersBLL(DatabaseContext DatabaseContext)
        {
            _DatabaseContext = DatabaseContext;
        }

        DatabaseContext _DatabaseContext;

        public void Dispose()
        {
            if (_DatabaseContext != null)
            {
                _DatabaseContext.Dispose();
                _DatabaseContext = null;
            }
            
            GC.SuppressFinalize(this);
        }

        partial void OnGroup_UsersSaving(Group_Users group_Users);

        partial void OnGroup_UsersCreating(Group_Users group_Users);
        partial void OnGroup_UsersCreated(Group_Users group_Users);

        partial void OnGroup_UsersUpdating(Group_Users group_Users);
        partial void OnGroup_UsersUpdated(Group_Users group_Users);

        partial void OnGroup_UsersSaved(Group_Users group_Users);

        partial void OnGroup_UsersDeleting(Group_Users group_Users);
        partial void OnGroup_UsersDeleted(Group_Users group_Users);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public Group_Users GetGroup_UsersByGroupUserID(int groupUserID)
        {
            //Validate Input
            if (groupUserID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("groupUserID");
            return (_DatabaseContext.Group_Users.FirstOrDefault(group_Users => group_Users.GroupUserID == groupUserID));
        }
			
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group_Users> GetGroup_UsersByGroupID(int groupID)
        {
            //Validate Input
            if (groupID.IsEmpty())
                return GetAllGroup_Users();
 
            return (from group_Users in _DatabaseContext.Group_Users
                    where groupID == null ? group_Users.GroupID == null : group_Users.GroupID == groupID
                    select group_Users).ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group_Users> GetGroup_UsersByGroupIDPaged(int groupID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (groupID.IsEmpty())
                return GetAllGroup_UsersPaged(orderBy, startRowIndex, maximumRows);

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "GroupUserID";
            
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
				
            return (
                    from group_Users in
                        _DatabaseContext.Group_Users
                        .DynamicOrderBy(orderBy)
                    where groupID == null ? group_Users.GroupID == null : group_Users.GroupID == groupID
                    select group_Users
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllGroup_UsersByGroupID(int groupID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (groupID.IsEmpty())
                return GetTotalCountForAllGroup_Users(orderBy, startRowIndex, maximumRows);
			
            return _DatabaseContext.Group_Users.Count(group_Users => groupID == null ? group_Users.GroupID == null : group_Users.GroupID == groupID);
        }
			
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group_Users> GetGroup_UsersByUserID(Guid userID)
        {
            //Validate Input
            if (userID.IsEmpty())
                return GetAllGroup_Users();
 
            return (from group_Users in _DatabaseContext.Group_Users
                    where userID == null ? group_Users.UserID == null : group_Users.UserID == userID
                    select group_Users).ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group_Users> GetGroup_UsersByUserIDPaged(Guid userID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (userID.IsEmpty())
                return GetAllGroup_UsersPaged(orderBy, startRowIndex, maximumRows);

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "GroupUserID";
            
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
				
            return (
                    from group_Users in
                        _DatabaseContext.Group_Users
                        .DynamicOrderBy(orderBy)
                    where userID == null ? group_Users.UserID == null : group_Users.UserID == userID
                    select group_Users
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllGroup_UsersByUserID(Guid userID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (userID.IsEmpty())
                return GetTotalCountForAllGroup_Users(orderBy, startRowIndex, maximumRows);
			
            return _DatabaseContext.Group_Users.Count(group_Users => userID == null ? group_Users.UserID == null : group_Users.UserID == userID);
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group_Users> GetAllGroup_Users()
        {
            return _DatabaseContext.Group_Users.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Group_Users> GetAllGroup_UsersPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "GroupUserID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from group_UsersList in 
                        _DatabaseContext.Group_Users
                        .DynamicOrderBy(orderBy)
                    select group_UsersList
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllGroup_Users(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.Group_Users.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual int CreateNewGroup_Users(Group_Users newGroup_Users)
        {
            // Validate Parameters 
            if (newGroup_Users == null)
                throw (new ArgumentNullException("newGroup_Users"));

	        // Apply business rules
            OnGroup_UsersSaving(newGroup_Users);
            OnGroup_UsersCreating(newGroup_Users);

            _DatabaseContext.Group_Users.AddObject(newGroup_Users);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No group_Users created!");

            // Apply business workflow
            OnGroup_UsersCreated(newGroup_Users);
            OnGroup_UsersSaved(newGroup_Users);

            return newGroup_Users.GroupUserID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateGroup_Users(Group_Users updatedGroup_Users)
        {
            // Validate Parameters
            if (updatedGroup_Users == null)
                throw (new ArgumentNullException("updatedGroup_Users"));

            // Validate Primary key value
            if (updatedGroup_Users.GroupUserID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("GroupUserID");

            // Apply business rules
            OnGroup_UsersSaving(updatedGroup_Users);
            OnGroup_UsersUpdating(updatedGroup_Users);

            //attaching and making ready for parsistance
            if (updatedGroup_Users.EntityState == EntityState.Detached)
                _DatabaseContext.Group_Users.Attach(updatedGroup_Users);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedGroup_Users, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No group_Users updated!");

            //Apply business workflow
            OnGroup_UsersUpdated(updatedGroup_Users);
            OnGroup_UsersSaved(updatedGroup_Users);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteGroup_Users(Group_Users group_UsersToBeDeleted)
        {
            //Validate Input
            if (group_UsersToBeDeleted == null)
                throw (new ArgumentNullException("group_UsersToBeDeleted"));

            // Validate Primary key value
            if (group_UsersToBeDeleted.GroupUserID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("GroupUserID");

            OnGroup_UsersSaving(group_UsersToBeDeleted);
            OnGroup_UsersDeleting(group_UsersToBeDeleted);

            if (group_UsersToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.Group_Users.Attach(group_UsersToBeDeleted);
			_DatabaseContext.Group_Users.DeleteObject(group_UsersToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No Group_Users deleted!");
            
            OnGroup_UsersDeleted(group_UsersToBeDeleted);
            OnGroup_UsersSaved(group_UsersToBeDeleted);

        }

        public void DeleteGroup_Users(List<int> groupUserIDsToDelete)
        {
            //Validate Input
            foreach (int groupUserID in groupUserIDsToDelete)
                if (groupUserID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("GroupUserID");

            List<Group_Users> group_UserssToBeDeleted = new List<Group_Users>();

            foreach (int groupUserID in groupUserIDsToDelete)
            {
                Group_Users group_Users = new Group_Users { GroupUserID = groupUserID };
                _DatabaseContext.Group_Users.Attach(group_Users);
				_DatabaseContext.Group_Users.DeleteObject(group_Users);
                group_UserssToBeDeleted.Add(group_Users);
                OnGroup_UsersDeleting(group_Users);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != groupUserIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more group_Users records have not been deleted.");
            foreach (Group_Users group_UsersToBeDeleted in group_UserssToBeDeleted)
                OnGroup_UsersDeleted(group_UsersToBeDeleted);
        }

        #endregion
	
	}
}