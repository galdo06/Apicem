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

namespace Eisk.BusinessLogicLayer
{
    public partial class ProjectBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Project> GetProjectByFilter(string project = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "editedDate desc";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<Project> projects = (
                    from projectList in
                        _DatabaseContext.Projects
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(project) || projectList.ProjectName.ToUpper().Trim().Contains(project.ToUpper().Trim()))
                    select projectList
                )
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList();

            return projects;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public   Project GetProjectByProjectId2(int projectID)
        {
            //Validate Input
            if (projectID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("projectID");
            Project project = (_DatabaseContext.Projects.FirstOrDefault(instance => instance.ProjectID == projectID));

           // var ass = project.ProjectCreator.UserName;
            return project;
        }

        public int GetTotalCountForAllProjects(string project = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            //if (string.IsNullOrEmpty(orderBy))
            //    orderBy = "projectName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<Project> projects = (
                    from projectList in
                        _DatabaseContext.Projects
                    where
                       (string.IsNullOrEmpty(project) || projectList.ProjectName.ToUpper().Contains(project.ToUpper()))
                    select projectList
                )
                .ToList();
            return projects.Count();
        }

    }
}
