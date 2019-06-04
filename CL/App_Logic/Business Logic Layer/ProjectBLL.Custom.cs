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
    public partial class ProjectBLL
    {
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public int GetTotalCountForAllProjectsInGroupByFilter(string project = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int groupID = 0)
        {
            var count = (int)HttpContext.Current.Items["projects_Count"];
            HttpContext.Current.Items.Remove("projects_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
            List<Project> projects = (
                from groupProjectList in
                    _DatabaseContext.Group_Projects
                where
                (groupProjectList.GroupID == groupID)
                select (groupProjectList.Project)
            )
            .Where
            (
                instance => (string.IsNullOrEmpty(project) || instance.ProjectInfoes.FirstOrDefault().ProjectName.ToUpper().Trim().Contains(project.ToUpper().Trim()))
            )
            .ToList();

            return projects.Count;
            */
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Project> GetProjectsInGroupByFilter(string project = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int groupID = 0)
        {
            //if (string.IsNullOrEmpty(orderBy))
            //    orderBy = "editedDate desc";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<Project> projects = (
                from groupProjectList in
                    _DatabaseContext.Group_Projects
                where
                (groupProjectList.GroupID == groupID)
                select (groupProjectList.Project)
            )
            .Where
            (
                instance2 => (string.IsNullOrEmpty(project) || instance2.ProjectInfoes.FirstOrDefault().ProjectName.ToUpper().Trim().Contains(project.ToUpper().Trim()))
            )
            .OrderBy(instance => instance.ProjectInfoes.FirstOrDefault().ProjectName)
            .OrderByDescending(instance => instance.EditedDate)
            .ToList();

            var retunrProjectsList = projects.Skip(startRowIndex).Take(maximumRows).ToList();

            HttpContext.Current.Items["projects_Count"] = projects.Count;

            return retunrProjectsList;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

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
                       (string.IsNullOrEmpty(project) || projectList.ProjectInfoes.FirstOrDefault().ProjectName.ToUpper().Trim().Equals(project.ToUpper().Trim()))
                    select projectList
                )
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList();

            return projects;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public Project GetProjectByProjectId2(int projectID)
        {
            //Validate Input
            if (projectID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("projectID");
            Project project = (_DatabaseContext.Projects.FirstOrDefault(instance => instance.ProjectID == projectID));

            if (project.ProjectInfoes.Count == 0)
            {
                ProjectInfo ProjectInfo = new ProjectInfo();
                ProjectInfo.ProjectID = project.ProjectID;

                ProjectInfo.ProjectReference.EntityKey = project.EntityKey;
                ProjectInfo.EditedDate = DateTime.Now;
                project.ProjectInfoes.Add(ProjectInfo);

                _DatabaseContext.SaveChanges();
            }

            var projectInfoes = project.ProjectInfoes;
            var projectInfoTreeLocations = project.ProjectInfoTreeLocations;
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
                       (string.IsNullOrEmpty(project) || projectList.ProjectInfoes.FirstOrDefault().ProjectName.ToUpper().Contains(project.ToUpper()))
                    select projectList
                )
                .ToList();
            return projects.Count();
        }

        public  Project CreteProject(string projectName, User user, City city, List<Group> groups)
        {
            Project project = new Project();

            project.CreatorUserID = user.UserID;
            project.CreatedDate = DateTime.Now;
            project.EditorUserID = user.UserID;
            project.EditedDate = DateTime.Now;

            new ProjectBLL().CreateNewProject(project);

            ProjectInfo projectInfo = new ProjectInfo();
            projectInfo.ProjectName = projectName;
            projectInfo.CityID = city.CityID;
            projectInfo.CityReference.EntityKey = city.EntityKey;

            projectInfo.ProjectID = project.ProjectID;
            projectInfo.ProjectReference.EntityKey = project.EntityKey;

            new ProjectInfoBLL().CreateNewProjectInfo(projectInfo);
            //

            // Group_Projects
            Group group = groups.First();

            Group_Projects group_Projects = new Eisk.BusinessEntities.Group_Projects();

            group_Projects.GroupID = group.GroupID;
            group_Projects.GroupReference.EntityKey = group.EntityKey;

            group_Projects.ProjectID = project.ProjectID;
            group_Projects.ProjectReference.EntityKey = project.EntityKey;

            new Group_ProjectsBLL().CreateNewGroup_Projects(group_Projects);
            //

            return project;
        }

        public void DeleteProjectAndObjectsByIDs(int projectsID)
        {
            List<int> projectsIDs = new List<int>();
            projectsIDs.Add(projectsID);

            DeleteProjectsAndObjectsByIDs(projectsIDs);
        }

        public void DeleteProjectAndObjects(Project project)
        {
            List<Project> projects = new List<Project>();
            projects.Add(project);

            DeleteProjectsAndObjects(projects);
        }

        public void DeleteProjectsAndObjectsByIDs(List<int> projectsIDs)
        {
            List<Project> projectsToDelete = new List<Project>();
            foreach (int projectID in projectsIDs)
            {
                projectsToDelete.Add(this.GetProjectByProjectID(projectID));
            }

            DeleteProjectsAndObjects(projectsToDelete);
        }

        public void DeleteProjectsAndObjects(List<Project> projectsToDelete)
        {
            foreach (var project in projectsToDelete)
            {                // Perimeter
                List<Int32> PerimeterIDsToDelete = new List<Int32>();
                List<Int32> PerimeterPointIDsToDelete = new List<Int32>();
                //

                // Tree
                List<Int32> DapIDsToDelete = new List<Int32>();
                List<Int32> TreeDetailIDsToDelete = new List<Int32>();
                List<Int32> ProjectOrganismsIDsToDelete = new List<Int32>();
                //

                // Project
                List<Int32> ProjectIDsToDelete = new List<Int32>();
                List<Int32> ProjectInfoIDsToDelete = new List<Int32>();
                List<Int32> ProjectInfoTreeLocationIDsToDelete = new List<Int32>();
                List<Int32> GroupProjectIDsToDelete = new List<Int32>();
                //
                //perform the actual delete

                foreach (ProjectInfoTreeLocation projectInfoTreeLocation in project.ProjectInfoTreeLocations)
                {
                    ProjectInfoTreeLocationIDsToDelete.Add(projectInfoTreeLocation.ProjectInfoTreeLocationID);
                }

                foreach (ProjectInfo projectInfo in project.ProjectInfoes)
                {
                    ProjectInfoIDsToDelete.Add(projectInfo.ProjectInfoID);
                }

                foreach (Project_Organisms project_Organism in project.Project_Organisms)
                {
                    foreach (TreeDetail treeDetail in project_Organism.TreeDetails)
                    {
                        foreach (Dap dap in treeDetail.Daps)
                        {
                            DapIDsToDelete.Add(dap.DapID);
                        }
                        TreeDetailIDsToDelete.Add(treeDetail.TreeDetailsID);
                    }
                    ProjectOrganismsIDsToDelete.Add(project_Organism.ProjectOrganismID);
                }

                foreach (Perimeter perimeter in project.Perimeters)
                {
                    foreach (PerimeterPoint perimeterPoint in perimeter.PerimeterPoints)
                    {
                        PerimeterPointIDsToDelete.Add(perimeterPoint.PerimeterPointID);
                    }
                    PerimeterIDsToDelete.Add(perimeter.PerimeterID);
                }

                foreach (Group_Projects group_Project in project.Group_Projects)
                {
                    GroupProjectIDsToDelete.Add(group_Project.GroupProjectID);
                }

                ProjectIDsToDelete.Add(project.ProjectID);

                // Perimeter
                new PerimeterPointBLL().DeletePerimeterPoints(PerimeterPointIDsToDelete);
                new PerimeterBLL().DeletePerimeters(PerimeterIDsToDelete);
                //

                // Tree
                new DapBLL().DeleteDaps(DapIDsToDelete);
                new TreeDetailBLL().DeleteTreeDetails(TreeDetailIDsToDelete);
                new Project_OrganismsBLL().DeleteProject_Organisms(ProjectOrganismsIDsToDelete);
                //

                // Project
                new ProjectInfoTreeLocationBLL().DeleteProjectInfoTreeLocations(ProjectInfoTreeLocationIDsToDelete);
                new ProjectInfoBLL().DeleteProjectInfoes(ProjectInfoIDsToDelete);
                new Group_ProjectsBLL().DeleteGroup_Projects(GroupProjectIDsToDelete);
                new ProjectBLL().DeleteProjects(ProjectIDsToDelete);
                //
            }
        }


    }
}
