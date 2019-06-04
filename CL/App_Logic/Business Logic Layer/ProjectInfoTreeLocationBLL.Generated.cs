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
	public partial class ProjectInfoTreeLocationBLL:IDisposable
	{
		#region Constructors, Dependency and Partial Method Declaration

        public ProjectInfoTreeLocationBLL() : this(new DatabaseContext()) { }

        public ProjectInfoTreeLocationBLL(DatabaseContext DatabaseContext)
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

        partial void OnProjectInfoTreeLocationSaving(ProjectInfoTreeLocation projectInfoTreeLocation);

        partial void OnProjectInfoTreeLocationCreating(ProjectInfoTreeLocation projectInfoTreeLocation);
        partial void OnProjectInfoTreeLocationCreated(ProjectInfoTreeLocation projectInfoTreeLocation);

        partial void OnProjectInfoTreeLocationUpdating(ProjectInfoTreeLocation projectInfoTreeLocation);
        partial void OnProjectInfoTreeLocationUpdated(ProjectInfoTreeLocation projectInfoTreeLocation);

        partial void OnProjectInfoTreeLocationSaved(ProjectInfoTreeLocation projectInfoTreeLocation);

        partial void OnProjectInfoTreeLocationDeleting(ProjectInfoTreeLocation projectInfoTreeLocation);
        partial void OnProjectInfoTreeLocationDeleted(ProjectInfoTreeLocation projectInfoTreeLocation);


        #endregion

        #region Get Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public ProjectInfoTreeLocation GetProjectInfoTreeLocationByProjectInfoTreeLocationID(int projectInfoTreeLocationID)
        {
            //Validate Input
            if (projectInfoTreeLocationID.IsInvalidKey())
               BusinessLayerHelper.ThrowErrorForInvalidDataKey("projectInfoTreeLocationID");
            return (_DatabaseContext.ProjectInfoTreeLocations.FirstOrDefault(projectInfoTreeLocation => projectInfoTreeLocation.ProjectInfoTreeLocationID == projectInfoTreeLocationID));
        }
			
		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<ProjectInfoTreeLocation> GetProjectInfoTreeLocationsByProjectID(int projectID)
        {
            //Validate Input
            if (projectID.IsEmpty())
                return GetAllProjectInfoTreeLocations();
 
            return (from projectInfoTreeLocation in _DatabaseContext.ProjectInfoTreeLocations
                    where projectID == null ? projectInfoTreeLocation.ProjectID == null : projectInfoTreeLocation.ProjectID == projectID
                    select projectInfoTreeLocation).ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<ProjectInfoTreeLocation> GetProjectInfoTreeLocationsByProjectIDPaged(int projectID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (projectID.IsEmpty())
                return GetAllProjectInfoTreeLocationsPaged(orderBy, startRowIndex, maximumRows);

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "ProjectInfoTreeLocationID";
            
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
				
            return (
                    from projectInfoTreeLocation in
                        _DatabaseContext.ProjectInfoTreeLocations
                        .DynamicOrderBy(orderBy)
                    where projectID == null ? projectInfoTreeLocation.ProjectID == null : projectInfoTreeLocation.ProjectID == projectID
                    select projectInfoTreeLocation
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllProjectInfoTreeLocationsByProjectID(int projectID, string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //Validate Input
            if (projectID.IsEmpty())
                return GetTotalCountForAllProjectInfoTreeLocations(orderBy, startRowIndex, maximumRows);
			
            return _DatabaseContext.ProjectInfoTreeLocations.Count(projectInfoTreeLocation => projectID == null ? projectInfoTreeLocation.ProjectID == null : projectInfoTreeLocation.ProjectID == projectID);
        }
			
				
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<ProjectInfoTreeLocation> GetAllProjectInfoTreeLocations()
        {
            return _DatabaseContext.ProjectInfoTreeLocations.ToList();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<ProjectInfoTreeLocation> GetAllProjectInfoTreeLocationsPaged(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "ProjectInfoTreeLocationID";
				
			if (startRowIndex < 0) 
				throw (new ArgumentOutOfRangeException("startRowIndex"));
				
			if (maximumRows < 0) 
				throw (new ArgumentOutOfRangeException("maximumRows"));
			
            return (
                    from projectInfoTreeLocationList in 
                        _DatabaseContext.ProjectInfoTreeLocations
                        .DynamicOrderBy(orderBy)
                    select projectInfoTreeLocationList
                    )
                    .Skip(startRowIndex)
                    .Take(maximumRows)
                    .ToList();
        }

        public int GetTotalCountForAllProjectInfoTreeLocations(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.ProjectInfoTreeLocations.Count();
        }

        #endregion

        #region Persistence (Create, Update, Delete) Methods

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public virtual int CreateNewProjectInfoTreeLocation(ProjectInfoTreeLocation newProjectInfoTreeLocation)
        {
            // Validate Parameters 
            if (newProjectInfoTreeLocation == null)
                throw (new ArgumentNullException("newProjectInfoTreeLocation"));

	        // Apply business rules
            OnProjectInfoTreeLocationSaving(newProjectInfoTreeLocation);
            OnProjectInfoTreeLocationCreating(newProjectInfoTreeLocation);

            _DatabaseContext.ProjectInfoTreeLocations.AddObject(newProjectInfoTreeLocation);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No projectInfoTreeLocation created!");

            // Apply business workflow
            OnProjectInfoTreeLocationCreated(newProjectInfoTreeLocation);
            OnProjectInfoTreeLocationSaved(newProjectInfoTreeLocation);

            return newProjectInfoTreeLocation.ProjectInfoTreeLocationID;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]
        public void UpdateProjectInfoTreeLocation(ProjectInfoTreeLocation updatedProjectInfoTreeLocation)
        {
            // Validate Parameters
            if (updatedProjectInfoTreeLocation == null)
                throw (new ArgumentNullException("updatedProjectInfoTreeLocation"));

            // Validate Primary key value
            if (updatedProjectInfoTreeLocation.ProjectInfoTreeLocationID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("ProjectInfoTreeLocationID");

            // Apply business rules
            OnProjectInfoTreeLocationSaving(updatedProjectInfoTreeLocation);
            OnProjectInfoTreeLocationUpdating(updatedProjectInfoTreeLocation);

            //attaching and making ready for parsistance
            if (updatedProjectInfoTreeLocation.EntityState == EntityState.Detached)
                _DatabaseContext.ProjectInfoTreeLocations.Attach(updatedProjectInfoTreeLocation);
			_DatabaseContext.ObjectStateManager.ChangeObjectState(updatedProjectInfoTreeLocation, System.Data.EntityState.Modified);//this line makes the code un-testable!
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No projectInfoTreeLocation updated!");

            //Apply business workflow
            OnProjectInfoTreeLocationUpdated(updatedProjectInfoTreeLocation);
            OnProjectInfoTreeLocationSaved(updatedProjectInfoTreeLocation);

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
        public void DeleteProjectInfoTreeLocation(ProjectInfoTreeLocation projectInfoTreeLocationToBeDeleted)
        {
            //Validate Input
            if (projectInfoTreeLocationToBeDeleted == null)
                throw (new ArgumentNullException("projectInfoTreeLocationToBeDeleted"));

            // Validate Primary key value
            if (projectInfoTreeLocationToBeDeleted.ProjectInfoTreeLocationID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("ProjectInfoTreeLocationID");

            OnProjectInfoTreeLocationSaving(projectInfoTreeLocationToBeDeleted);
            OnProjectInfoTreeLocationDeleting(projectInfoTreeLocationToBeDeleted);

            if (projectInfoTreeLocationToBeDeleted.EntityState == EntityState.Detached)
             _DatabaseContext.ProjectInfoTreeLocations.Attach(projectInfoTreeLocationToBeDeleted);
			_DatabaseContext.ProjectInfoTreeLocations.DeleteObject(projectInfoTreeLocationToBeDeleted);
            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows == 0) 
                throw new DataNotUpdatedException("No ProjectInfoTreeLocation deleted!");
            
            OnProjectInfoTreeLocationDeleted(projectInfoTreeLocationToBeDeleted);
            OnProjectInfoTreeLocationSaved(projectInfoTreeLocationToBeDeleted);

        }

        public void DeleteProjectInfoTreeLocations(List<int> projectInfoTreeLocationIDsToDelete)
        {
            //Validate Input
            foreach (int projectInfoTreeLocationID in projectInfoTreeLocationIDsToDelete)
                if (projectInfoTreeLocationID.IsInvalidKey())
                    BusinessLayerHelper.ThrowErrorForInvalidDataKey("ProjectInfoTreeLocationID");

            List<ProjectInfoTreeLocation> projectInfoTreeLocationsToBeDeleted = new List<ProjectInfoTreeLocation>();

            foreach (int projectInfoTreeLocationID in projectInfoTreeLocationIDsToDelete)
            {
                ProjectInfoTreeLocation projectInfoTreeLocation = new ProjectInfoTreeLocation { ProjectInfoTreeLocationID = projectInfoTreeLocationID };
                _DatabaseContext.ProjectInfoTreeLocations.Attach(projectInfoTreeLocation);
				_DatabaseContext.ProjectInfoTreeLocations.DeleteObject(projectInfoTreeLocation);
                projectInfoTreeLocationsToBeDeleted.Add(projectInfoTreeLocation);
                OnProjectInfoTreeLocationDeleting(projectInfoTreeLocation);
            }

            int numberOfAffectedRows = _DatabaseContext.SaveChanges();
            if (numberOfAffectedRows != projectInfoTreeLocationIDsToDelete.Count) 
                throw new DataNotUpdatedException("One or more projectInfoTreeLocation records have not been deleted.");
            foreach (ProjectInfoTreeLocation projectInfoTreeLocationToBeDeleted in projectInfoTreeLocationsToBeDeleted)
                OnProjectInfoTreeLocationDeleted(projectInfoTreeLocationToBeDeleted);
        }

        #endregion
	
	}
}