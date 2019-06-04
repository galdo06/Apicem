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
	public partial class CommonNameBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public CommonNameBLL() : this(new DatabaseContext()) { }

        public CommonNameBLL(DatabaseContext DatabaseContext)
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

        partial void OnCommonNameSaving(CommonName commonName);

        partial void OnCommonNameCreating(CommonName commonName);
        partial void OnCommonNameCreated(CommonName commonName);

        partial void OnCommonNameUpdating(CommonName commonName);
        partial void OnCommonNameUpdated(CommonName commonName);

        partial void OnCommonNameSaved(CommonName commonName);

        partial void OnCommonNameDeleting(CommonName commonName);
        partial void OnCommonNameDeleted(CommonName commonName);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public CommonName GetCommonNameByCommonNameID(int commonNameID)
        {
            //Validate Input
            if (commonNameID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("commonNameID");
            return (_DatabaseContext.CommonNames.FirstOrDefault(commonName => commonName.CommonNameID == commonNameID));
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<CommonName> GetAllCommonNames()
        {
            return _DatabaseContext.CommonNames.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<CommonName> GetAllCommonNamesPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "CommonNameID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from commonNameList in 
                        _DatabaseContext.CommonNames
                        .DynamicOrderBy(orderBy)
                    select commonNameList
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllCommonNames(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.CommonNames.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual int CreateNewCommonName(CommonName newCommonName)
        {
            // Validate Parameters 
            if (newCommonName == null)
                throw (new ArgumentNullException("newCommonName"));

	        // Apply business rules
            OnCommonNameSaving(newCommonName);
            OnCommonNameCreating(newCommonName);

            _DatabaseContext.CommonNames.AddObject(newCommonName);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No commonName created!");

            // Apply business workflow
            OnCommonNameCreated(newCommonName);
            OnCommonNameSaved(newCommonName);

            return newCommonName.CommonNameID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateCommonName(CommonName updatedCommonName)
        {
            // Validate Parameters
            if (updatedCommonName == null)
                throw (new ArgumentNullException("updatedCommonName"));

            // Validate Primary key value
            if (updatedCommonName.CommonNameID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("CommonNameID");

            // Apply business rules
            OnCommonNameSaving(updatedCommonName);
            OnCommonNameUpdating(updatedCommonName);

            //attaching and making ready for parsistance
            if (updatedCommonName.EntityState == EntityState.Detached)
                _DatabaseContext.CommonNames.Attach(updatedCommonName);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedCommonName, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No commonName updated!");

            //Apply business workflow
            OnCommonNameUpdated(updatedCommonName);
            OnCommonNameSaved(updatedCommonName);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteCommonName(CommonName commonNameToBeDeleted)
        {
            //Validate Input
            if (commonNameToBeDeleted == null)
                throw (new ArgumentNullException("commonNameToBeDeleted"));

            // Validate Primary key value
            if (commonNameToBeDeleted.CommonNameID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("CommonNameID");

            OnCommonNameSaving(commonNameToBeDeleted);
            OnCommonNameDeleting(commonNameToBeDeleted);

            if (commonNameToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.CommonNames.Attach(commonNameToBeDeleted);
			_DatabaseContext.CommonNames.DeleteObject(commonNameToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No CommonName deleted!");
            
            OnCommonNameDeleted(commonNameToBeDeleted);
            OnCommonNameSaved(commonNameToBeDeleted);

        }

        public void DeleteCommonNames(List<int> commonNameIDsToDelete)
        {
            //Validate Input
            foreach (int commonNameID in commonNameIDsToDelete)
                if (commonNameID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("CommonNameID");

            List<CommonName> commonNamesToBeDeleted = new List<CommonName>();

            foreach (int commonNameID in commonNameIDsToDelete)
            {
                CommonName commonName = new CommonName { CommonNameID = commonNameID };
                _DatabaseContext.CommonNames.Attach(commonName);
				_DatabaseContext.CommonNames.DeleteObject(commonName);
                commonNamesToBeDeleted.Add(commonName);
                OnCommonNameDeleting(commonName);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != commonNameIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more commonName records have not been deleted.");
            foreach (CommonName commonNameToBeDeleted in commonNamesToBeDeleted)
                OnCommonNameDeleted(commonNameToBeDeleted);
        }

        #endregion
	
	}
}