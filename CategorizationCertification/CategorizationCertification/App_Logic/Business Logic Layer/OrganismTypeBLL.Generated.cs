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
	public partial class OrganismTypeBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public OrganismTypeBLL() : this(new DatabaseContext()) { }

        public OrganismTypeBLL(DatabaseContext DatabaseContext)
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

        partial void OnOrganismTypeSaving(OrganismType organismType);

        partial void OnOrganismTypeCreating(OrganismType organismType);
        partial void OnOrganismTypeCreated(OrganismType organismType);

        partial void OnOrganismTypeUpdating(OrganismType organismType);
        partial void OnOrganismTypeUpdated(OrganismType organismType);

        partial void OnOrganismTypeSaved(OrganismType organismType);

        partial void OnOrganismTypeDeleting(OrganismType organismType);
        partial void OnOrganismTypeDeleted(OrganismType organismType);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public OrganismType GetOrganismTypeByOrganismTypeID(int organismTypeID)
        {
            //Validate Input
            if (organismTypeID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("organismTypeID");
            return (_DatabaseContext.OrganismTypes.FirstOrDefault(organismType => organismType.OrganismTypeID == organismTypeID));
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<OrganismType> GetAllOrganismTypes()
        {
            return _DatabaseContext.OrganismTypes.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<OrganismType> GetAllOrganismTypesPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrganismTypeID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from organismType in 
                        _DatabaseContext.OrganismTypes
                        .DynamicOrderBy(orderBy)
                    select organismType
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllOrganismTypes(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.OrganismTypes.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual int CreateNewOrganismType(OrganismType newOrganismType)
        {
            // Validate Parameters 
            if (newOrganismType == null)
                throw (new ArgumentNullException("newOrganismType"));

	        // Apply business rules
            OnOrganismTypeSaving(newOrganismType);
            OnOrganismTypeCreating(newOrganismType);

            _DatabaseContext.OrganismTypes.AddObject(newOrganismType);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No organismType created!");

            // Apply business workflow
            OnOrganismTypeCreated(newOrganismType);
            OnOrganismTypeSaved(newOrganismType);

            return newOrganismType.OrganismTypeID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateOrganismType(OrganismType updatedOrganismType)
        {
            // Validate Parameters
            if (updatedOrganismType == null)
                throw (new ArgumentNullException("updatedOrganismType"));

            // Validate Primary key value
            if (updatedOrganismType.OrganismTypeID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("OrganismTypeID");

            // Apply business rules
            OnOrganismTypeSaving(updatedOrganismType);
            OnOrganismTypeUpdating(updatedOrganismType);

            //attaching and making ready for parsistance
            if (updatedOrganismType.EntityState == EntityState.Detached)
                _DatabaseContext.OrganismTypes.Attach(updatedOrganismType);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedOrganismType, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No organismType updated!");

            //Apply business workflow
            OnOrganismTypeUpdated(updatedOrganismType);
            OnOrganismTypeSaved(updatedOrganismType);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteOrganismType(OrganismType organismTypeToBeDeleted)
        {
            //Validate Input
            if (organismTypeToBeDeleted == null)
                throw (new ArgumentNullException("organismTypeToBeDeleted"));

            // Validate Primary key value
            if (organismTypeToBeDeleted.OrganismTypeID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("OrganismTypeID");

            OnOrganismTypeSaving(organismTypeToBeDeleted);
            OnOrganismTypeDeleting(organismTypeToBeDeleted);

            if (organismTypeToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.OrganismTypes.Attach(organismTypeToBeDeleted);
			_DatabaseContext.OrganismTypes.DeleteObject(organismTypeToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No OrganismType deleted!");
            
            OnOrganismTypeDeleted(organismTypeToBeDeleted);
            OnOrganismTypeSaved(organismTypeToBeDeleted);

        }

        public void DeleteOrganismTypes(List<int> organismTypeIDsToDelete)
        {
            //Validate Input
            foreach (int organismTypeID in organismTypeIDsToDelete)
                if (organismTypeID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("OrganismTypeID");

            List<OrganismType> organismTypesToBeDeleted = new List<OrganismType>();

            foreach (int organismTypeID in organismTypeIDsToDelete)
            {
                OrganismType organismType = new OrganismType { OrganismTypeID = organismTypeID };
                _DatabaseContext.OrganismTypes.Attach(organismType);
				_DatabaseContext.OrganismTypes.DeleteObject(organismType);
                organismTypesToBeDeleted.Add(organismType);
                OnOrganismTypeDeleting(organismType);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != organismTypeIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more organismType records have not been deleted.");
            foreach (OrganismType organismTypeToBeDeleted in organismTypesToBeDeleted)
                OnOrganismTypeDeleted(organismTypeToBeDeleted);
        }

        #endregion
	
	}
}
