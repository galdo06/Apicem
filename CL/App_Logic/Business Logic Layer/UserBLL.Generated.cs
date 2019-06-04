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
	public partial class UserBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public UserBLL() : this(new DatabaseContext()) { }

        public UserBLL(DatabaseContext DatabaseContext)
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

        partial void OnUserSaving(User user);

        partial void OnUserCreating(User user);
        partial void OnUserCreated(User user);

        partial void OnUserUpdating(User user);
        partial void OnUserUpdated(User user);

        partial void OnUserSaved(User user);

        partial void OnUserDeleting(User user);
        partial void OnUserDeleted(User user);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public User GetUserByUserID(Guid userID)
        {
            //Validate Input
            if (userID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("userID");
            return (_DatabaseContext.Users.FirstOrDefault(user => user.UserID == userID));
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<User> GetAllUsers()
        {
            return _DatabaseContext.Users.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<User> GetAllUsersPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "UserID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from userList in 
                        _DatabaseContext.Users
                        .DynamicOrderBy(orderBy)
                    select userList
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllUsers(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.Users.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual Guid CreateNewUser(User newUser)
        {
            // Validate Parameters 
            if (newUser == null)
                throw (new ArgumentNullException("newUser"));

	        // Apply business rules
            OnUserSaving(newUser);
            OnUserCreating(newUser);

            _DatabaseContext.Users.AddObject(newUser);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No user created!");

            // Apply business workflow
            OnUserCreated(newUser);
            OnUserSaved(newUser);

            return newUser.UserID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateUser(User updatedUser)
        {
            // Validate Parameters
            if (updatedUser == null)
                throw (new ArgumentNullException("updatedUser"));

            // Validate Primary key value
            if (updatedUser.UserID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("UserID");

            // Apply business rules
            OnUserSaving(updatedUser);
            OnUserUpdating(updatedUser);

            //attaching and making ready for parsistance
            if (updatedUser.EntityState == EntityState.Detached)
                _DatabaseContext.Users.Attach(updatedUser);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedUser, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No user updated!");

            //Apply business workflow
            OnUserUpdated(updatedUser);
            OnUserSaved(updatedUser);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteUser(User userToBeDeleted)
        {
            //Validate Input
            if (userToBeDeleted == null)
                throw (new ArgumentNullException("userToBeDeleted"));

            // Validate Primary key value
            if (userToBeDeleted.UserID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("UserID");

            OnUserSaving(userToBeDeleted);
            OnUserDeleting(userToBeDeleted);

            if (userToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.Users.Attach(userToBeDeleted);
			_DatabaseContext.Users.DeleteObject(userToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No User deleted!");
            
            OnUserDeleted(userToBeDeleted);
            OnUserSaved(userToBeDeleted);

        }

        public void DeleteUsers(List<Guid> userIDsToDelete)
        {
            //Validate Input
            foreach (Guid userID in userIDsToDelete)
                if (userID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("UserID");

            List<User> usersToBeDeleted = new List<User>();

            foreach (Guid userID in userIDsToDelete)
            {
                User user = new User { UserID = userID };
                _DatabaseContext.Users.Attach(user);
				_DatabaseContext.Users.DeleteObject(user);
                usersToBeDeleted.Add(user);
                OnUserDeleting(user);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != userIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more user records have not been deleted.");
            foreach (User userToBeDeleted in usersToBeDeleted)
                OnUserDeleted(userToBeDeleted);
        }

        #endregion
	
	}
}