using Comments.Web.Filter;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Web.Http;
using WS.Authorizers;
using OAuth2.Mvc;
using System;
using WS.Models;
using Ws.Validations;
using System.Web;
using Eisk.DataAccessLayer;
using Eisk.Web.App_Logic.Helpers;

namespace WS.Controllers
{
    #region ModelHelpers

    public class ProjectIDModel
    {
        [ProjectIDValidation]
        public int? ProjectID { get; set; }
    }

    #endregion

    [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
    public class ProjectController : ApiController
    {
        #region Api Calls

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public object Get([FromBody]ProjectIDModel projectIDModel)
        {
            var project = (Project)HttpContext.Current.Items["project"];

            return ProjectModel.GetProjectObject(project);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public List<object> GetAll()
        {
            User user = (User)HttpContext.Current.Items["user"];
            List<Group> groups = UserModel.GetUserGroups(user);
            List<object> projects = groups.First().Group_Projects.OrderByDescending(instance2 => instance2.Project.EditedDate).Select(instance => ProjectModel.GetProjectObject(instance.Project)).ToList();

            return projects;
        }

        [HttpPost]
        public object Post([FromBody]ProjectModel newProject)
        {
            User user = (User)HttpContext.Current.Items["user"];
            City city = (City)HttpContext.Current.Items["city"];
            List<Group> groups = (List<Group>)HttpContext.Current.Items["groups"];

            Project project = new Project();
            project.CreatedDate = DateTime.Now;
            project.CreatorUserID = user.UserID;
            project.EditedDate = DateTime.Now;
            project.EditorUserID = user.UserID;

            new ProjectBLL().CreateNewProject(project);

            ProjectInfo projectInfo = new ProjectInfo();
            projectInfo.ProjectName = newProject.ProjectName;
            projectInfo.Client = newProject.Client;
            projectInfo.Address1 = newProject.Address1;
            projectInfo.Address2 = newProject.Address2;         
            projectInfo.CityID = newProject.CityID;
            projectInfo.CityReference.EntityKey = city.EntityKey;
            projectInfo.State = newProject.State;
            projectInfo.ZipCode = newProject.ZipCode != null ? newProject.ZipCode.Replace("-", "") : "";
            projectInfo.Description = newProject.Description != null ? newProject.Description.Substring(0, Math.Min(newProject.Description.Length, 5000)) : "";
            projectInfo.Comments = newProject.Comments != null ? newProject.Comments.Substring(0, Math.Min(newProject.Comments.Length, 5000)) : "";

            projectInfo.ProjectID = project.ProjectID;
            projectInfo.ProjectReference.EntityKey = project.EntityKey;

            new ProjectInfoBLL().CreateNewProjectInfo(projectInfo);

            ProjectInfoTreeLocation projectInfoTreeLocation = new ProjectInfoTreeLocation();
            projectInfoTreeLocation.Acres = newProject.Acres;
            projectInfoTreeLocation.DistanceBetweenTrees = newProject.DistanceBetweenTrees;
            projectInfoTreeLocation.Lots1 = newProject.Lots;
            projectInfoTreeLocation.Parkings = newProject.Parkings;

            if ((newProject.Lat == null || newProject.Lat == 0) || (newProject.Lon == null || newProject.Lon == 0))
            {
                newProject.Lat = city.Lat;
                newProject.Lon = city.Lon;
            }

            projectInfoTreeLocation.Lat = newProject.Lat;
            projectInfoTreeLocation.Lon = newProject.Lon;

            Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(projectInfoTreeLocation.Lon.ToString(), projectInfoTreeLocation.Lat.ToString(), @"~/Javascript/");
            projectInfoTreeLocation.X = Convert.ToDecimal(anewpointObj["x"]);
            projectInfoTreeLocation.Y = Convert.ToDecimal(anewpointObj["y"]);

            projectInfoTreeLocation.ProjectID = project.ProjectID;
            projectInfoTreeLocation.ProjectReference.EntityKey = project.EntityKey;

            new ProjectInfoTreeLocationBLL().CreateNewProjectInfoTreeLocation(projectInfoTreeLocation);

            // Group_Projects
            Group group = groups[0];

            Group_Projects group_Projects = new Eisk.BusinessEntities.Group_Projects();

            group_Projects.GroupID = group.GroupID;
            group_Projects.GroupReference.EntityKey = group.EntityKey;

            group_Projects.ProjectID = project.ProjectID;
            group_Projects.ProjectReference.EntityKey = project.EntityKey;

            new Group_ProjectsBLL().CreateNewGroup_Projects(group_Projects);
            //

            //   Project project = new ProjectBLL().CreateNewProject(newProject.ProjectName, user, city, groups);

            return ProjectModel.GetProjectObject(project);
        }

        [HttpPost]
        public object Update([FromBody]ProjectModel_Update projectUpdate)
        {
            User user = (User)HttpContext.Current.Items["user"];
            City city = (City)HttpContext.Current.Items["city"];
            List<Group> groups = (List<Group>)HttpContext.Current.Items["groups"];

            var project = (Project)HttpContext.Current.Items["project"];

            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                // ProjectInfo
                ProjectInfo projectInfo = _DatabaseContext.ProjectInfoes.First(instance => instance.ProjectID == projectUpdate.ProjectID);

                projectInfo.ProjectName = projectUpdate.ProjectName;
                projectInfo.Client = projectUpdate.Client;
                projectInfo.Address1 = projectUpdate.Address1;
                projectInfo.Address2 = projectUpdate.Address2;
                projectInfo.CityID = projectUpdate.CityID;
                projectInfo.State = projectUpdate.State;
                projectInfo.ZipCode = !string.IsNullOrWhiteSpace(projectUpdate.ZipCode) ? projectUpdate.ZipCode.Replace("-", "") : null;
                projectInfo.Description = projectUpdate.Description;
                projectInfo.Comments = projectUpdate.Comments;

                projectInfo.EditedDate = DateTime.Now;
                //

                // ProjectInfoTreeLocation
                ProjectInfoTreeLocation projectInfoTreeLocation = _DatabaseContext.ProjectInfoTreeLocations.First(instance => instance.ProjectID == projectUpdate.ProjectID);
                projectInfoTreeLocation.Acres = projectUpdate.Acres;
                projectInfoTreeLocation.DistanceBetweenTrees = projectUpdate.DistanceBetweenTrees;
                projectInfoTreeLocation.Lots1 = projectUpdate.Lots;
                projectInfoTreeLocation.Parkings = projectUpdate.Parkings;

                if ((projectUpdate.Lat == null || projectUpdate.Lat == 0) || (projectUpdate.Lon == null || projectUpdate.Lon == 0))
                {
                    projectUpdate.Lat = city.Lat;
                    projectUpdate.Lon = city.Lon;
                }

                projectInfoTreeLocation.Lat = projectUpdate.Lat;
                projectInfoTreeLocation.Lon = projectUpdate.Lon;

                Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(projectInfoTreeLocation.Lon.ToString(), projectInfoTreeLocation.Lat.ToString(), @"~/Javascript/");
                projectInfoTreeLocation.X = Convert.ToDecimal(anewpointObj["x"]);
                projectInfoTreeLocation.Y = Convert.ToDecimal(anewpointObj["y"]);
                //

                // Project
                project = _DatabaseContext.Projects.First(instance => instance.ProjectID == project.ProjectID);

                project.EditedDate = DateTime.Now;
                project.EditorUserID = user.UserID;
                //

                _DatabaseContext.SaveChanges();
            }

            project = new ProjectBLL().GetProjectByProjectID(project.ProjectID);

            return ProjectModel.GetProjectObject(project);
        }

        [HttpDelete]
        public object Delete([FromBody]ProjectIDModel projectIDModel)
        {
            var project = (Project)HttpContext.Current.Items["project"];

            new ProjectBLL().DeleteProjectAndObjects(project);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        #endregion

    }
}