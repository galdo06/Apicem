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
	public partial class Notification_UserBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public Notification_UserBLL() : this(new DatabaseContext()) { }

        public Notification_UserBLL(DatabaseContext DatabaseContext)
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

        partial void OnNotification_UserSaving(Notification_User notification_User);

        partial void OnNotification_UserCreating(Notification_User notification_User);
        partial void OnNotification_UserCreated(Notification_User notification_User);

        partial void OnNotification_UserUpdating(Notification_User notification_User);
        partial void OnNotification_UserUpdated(Notification_User notification_User);

        partial void OnNotification_UserSaved(Notification_User notification_User);

        partial void OnNotification_UserDeleting(Notification_User notification_User);
        partial void OnNotification_UserDeleted(Notification_User notification_User);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public Notification_User GetNotification_UserByNotificationUserID(int notificationUserID)
        {
            //Validate Input
            if (notificationUserID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("notificationUserID");
            return (_DatabaseContext.Notification_User.FirstOrDefault(notification_User => notification_User.NotificationUserID == notificationUserID));
        }
			
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetNotification_UserByGroupID(int? groupID)
        {
            //Validate Input
            if (groupID.IsEmpty())
                return GetAllNotification_User();
 
            return (from notification_User in _DatabaseContext.Notification_User
                    where groupID == null ? notification_User.GroupID == null : notification_User.GroupID == groupID
                    select notification_User).ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetNotification_UserByGroupIDPaged(int? groupID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (groupID.IsEmpty())
                return GetAllNotification_UserPaged(orderBy, startRowIndex, maximumRows);

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "NotificationUserID";
            
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
				
            return (
                    from notification_User in
                        _DatabaseContext.Notification_User
                        .DynamicOrderBy(orderBy)
                    where groupID == null ? notification_User.GroupID == null : notification_User.GroupID == groupID
                    select notification_User
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllNotification_UserByGroupID(int? groupID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (groupID.IsEmpty())
                return GetTotalCountForAllNotification_User(orderBy, startRowIndex, maximumRows);
			
            return _DatabaseContext.Notification_User.Count(notification_User => groupID == null ? notification_User.GroupID == null : notification_User.GroupID == groupID);
        }
			
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetNotification_UserByNotificationID(int? notificationID)
        {
            //Validate Input
            if (notificationID.IsEmpty())
                return GetAllNotification_User();
 
            return (from notification_User in _DatabaseContext.Notification_User
                    where notificationID == null ? notification_User.NotificationID == null : notification_User.NotificationID == notificationID
                    select notification_User).ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetNotification_UserByNotificationIDPaged(int? notificationID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (notificationID.IsEmpty())
                return GetAllNotification_UserPaged(orderBy, startRowIndex, maximumRows);

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "NotificationUserID";
            
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
				
            return (
                    from notification_User in
                        _DatabaseContext.Notification_User
                        .DynamicOrderBy(orderBy)
                    where notificationID == null ? notification_User.NotificationID == null : notification_User.NotificationID == notificationID
                    select notification_User
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllNotification_UserByNotificationID(int? notificationID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (notificationID.IsEmpty())
                return GetTotalCountForAllNotification_User(orderBy, startRowIndex, maximumRows);
			
            return _DatabaseContext.Notification_User.Count(notification_User => notificationID == null ? notification_User.NotificationID == null : notification_User.NotificationID == notificationID);
        }
			
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetNotification_UserByUserID(Guid? userID)
        {
            //Validate Input
            if (userID.IsEmpty())
                return GetAllNotification_User();
 
            return (from notification_User in _DatabaseContext.Notification_User
                    where userID == null ? notification_User.UserID == null : notification_User.UserID == userID
                    select notification_User).ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetNotification_UserByUserIDPaged(Guid? userID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (userID.IsEmpty())
                return GetAllNotification_UserPaged(orderBy, startRowIndex, maximumRows);

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "NotificationUserID";
            
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
				
            return (
                    from notification_User in
                        _DatabaseContext.Notification_User
                        .DynamicOrderBy(orderBy)
                    where userID == null ? notification_User.UserID == null : notification_User.UserID == userID
                    select notification_User
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllNotification_UserByUserID(Guid? userID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (userID.IsEmpty())
                return GetTotalCountForAllNotification_User(orderBy, startRowIndex, maximumRows);
			
            return _DatabaseContext.Notification_User.Count(notification_User => userID == null ? notification_User.UserID == null : notification_User.UserID == userID);
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetAllNotification_User()
        {
            return _DatabaseContext.Notification_User.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Notification_User> GetAllNotification_UserPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "NotificationUserID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from notification_UserList in 
                        _DatabaseContext.Notification_User
                        .DynamicOrderBy(orderBy)
                    select notification_UserList
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllNotification_User(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.Notification_User.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual int CreateNewNotification_User(Notification_User newNotification_User)
        {
            // Validate Parameters 
            if (newNotification_User == null)
                throw (new ArgumentNullException("newNotification_User"));

	        // Apply business rules
            OnNotification_UserSaving(newNotification_User);
            OnNotification_UserCreating(newNotification_User);

            _DatabaseContext.Notification_User.AddObject(newNotification_User);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No notification_User created!");

            // Apply business workflow
            OnNotification_UserCreated(newNotification_User);
            OnNotification_UserSaved(newNotification_User);

            return newNotification_User.NotificationUserID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateNotification_User(Notification_User updatedNotification_User)
        {
            // Validate Parameters
            if (updatedNotification_User == null)
                throw (new ArgumentNullException("updatedNotification_User"));

            // Validate Primary key value
            if (updatedNotification_User.NotificationUserID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("NotificationUserID");

            // Apply business rules
            OnNotification_UserSaving(updatedNotification_User);
            OnNotification_UserUpdating(updatedNotification_User);

            //attaching and making ready for parsistance
            if (updatedNotification_User.EntityState == EntityState.Detached)
                _DatabaseContext.Notification_User.Attach(updatedNotification_User);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedNotification_User, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No notification_User updated!");

            //Apply business workflow
            OnNotification_UserUpdated(updatedNotification_User);
            OnNotification_UserSaved(updatedNotification_User);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteNotification_User(Notification_User notification_UserToBeDeleted)
        {
            //Validate Input
            if (notification_UserToBeDeleted == null)
                throw (new ArgumentNullException("notification_UserToBeDeleted"));

            // Validate Primary key value
            if (notification_UserToBeDeleted.NotificationUserID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("NotificationUserID");

            OnNotification_UserSaving(notification_UserToBeDeleted);
            OnNotification_UserDeleting(notification_UserToBeDeleted);

            if (notification_UserToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.Notification_User.Attach(notification_UserToBeDeleted);
			_DatabaseContext.Notification_User.DeleteObject(notification_UserToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No Notification_User deleted!");
            
            OnNotification_UserDeleted(notification_UserToBeDeleted);
            OnNotification_UserSaved(notification_UserToBeDeleted);

        }

        public void DeleteNotification_User(List<int> notificationUserIDsToDelete)
        {
            //Validate Input
            foreach (int notificationUserID in notificationUserIDsToDelete)
                if (notificationUserID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("NotificationUserID");

            List<Notification_User> notification_UsersToBeDeleted = new List<Notification_User>();

            foreach (int notificationUserID in notificationUserIDsToDelete)
            {
                Notification_User notification_User = new Notification_User { NotificationUserID = notificationUserID };
                _DatabaseContext.Notification_User.Attach(notification_User);
				_DatabaseContext.Notification_User.DeleteObject(notification_User);
                notification_UsersToBeDeleted.Add(notification_User);
                OnNotification_UserDeleting(notification_User);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != notificationUserIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more notification_User records have not been deleted.");
            foreach (Notification_User notification_UserToBeDeleted in notification_UsersToBeDeleted)
                OnNotification_UserDeleted(notification_UserToBeDeleted);
        }

        #endregion
	
	}
}
