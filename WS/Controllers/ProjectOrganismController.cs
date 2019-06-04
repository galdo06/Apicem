using Comments.Web.Filter;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using Eisk.DataAccessLayer;
using Eisk.Web.App_Logic.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ws.Validations;
using WS.Authorizers;
using WS.Models;

namespace Ws.Controllers
{

    #region ModelHelpers

    public class ProjectOrganismIDModel
    {
        [ProjectOrganismIDValidationAttribute]
        public int? ProjectOrganismID { get; set; }
    }

    public class ProjectIDModel
    {
        [ProjectIDValidationAttribute]
        public int? ProjectID { get; set; }

    }

    #endregion

    [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
    public class ProjectOrganismController : ApiController
    {
        #region Api Calls

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public object Get([FromBody]ProjectOrganismIDModel projectOrganismIDModel)
        {
            Project_Organisms projectOrganism = (Project_Organisms)HttpContext.Current.Items["projectOrganism"];

            return ProjectOrganismModel.GetProjectOrganismObject(projectOrganism);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public List<object> GetAll([FromBody]ProjectIDModel projectIDModel)
        {
            var project = (Project)HttpContext.Current.Items["project"];
            //var groups = (List<Group>)HttpContext.Current.Items["groups"];

            return new Project_OrganismsBLL().GetAllProject_Organisms()
                .Where(instance => instance.ProjectID == project.ProjectID)
                .OrderByDescending(instance3 => instance3.TreeDetails.First().Number)
                .Select(instance2 => ProjectOrganismModel.GetProjectOrganismObject(instance2))
                .ToList();
        }

        [HttpPost]
        public object Post([FromBody]ProjectOrganismModel newProjectOrganism)
        {
            var organism = (Organism)HttpContext.Current.Items["organism"];
            var project = (Project)HttpContext.Current.Items["project"];
            var groups = (List<Group>)HttpContext.Current.Items["groups"];
            var user = (User)HttpContext.Current.Items["user"];
            var actionProposed = (ActionProposed)HttpContext.Current.Items["actionProposed"];
            var condition = (Condition)HttpContext.Current.Items["condition"];
            var daps = (int[])HttpContext.Current.Items["daps"];

            if (string.IsNullOrEmpty(newProjectOrganism.Commentary))
                newProjectOrganism.Commentary = "";

            TreeDetail treeDetail = new TreeDetailBLL().CreateProject_Organism(
                newProjectOrganism.Varas,
                newProjectOrganism.Height,
                newProjectOrganism.Lat,
                newProjectOrganism.Lon,
                daps, // newProjectOrganism.Daps.Select(instance => instance.DapValue).ToArray(), 
                actionProposed,
                condition,
                newProjectOrganism.Commentary,
                organism,
                project,
                user);

            return ProjectOrganismModel.GetProjectOrganismObject(treeDetail);
        }

        [HttpPost]
        public object Update([FromBody]ProjectOrganismUpdateModel projectOrganismUpdate)
        {
            //var projectOrganism = (Project_Organisms)HttpContext.Current.Items["projectOrganism"];
            //var organism = (Organism)HttpContext.Current.Items["organism"];
            var project = (Project)HttpContext.Current.Items["project"];
            var groups = (List<Group>)HttpContext.Current.Items["groups"];
            var user = (User)HttpContext.Current.Items["user"];
            //var actionProposed = (ActionProposed)HttpContext.Current.Items["actionProposed"];
            //var condition = (Condition)HttpContext.Current.Items["condition"];
            var daps = (int[])HttpContext.Current.Items["daps"];
            var varas = (int)HttpContext.Current.Items["varas"];


            using (DatabaseContext _DatabaseContext = new DatabaseContext())
            {
                // TreeDetail
                Project_Organisms projectOrganism = _DatabaseContext.Project_Organisms.First(instance => instance.ProjectOrganismID == projectOrganismUpdate.ProjectOrganismID);
                TreeDetail treeDetail = _DatabaseContext.TreeDetails.First(instance => instance.ProjectOrganismID == projectOrganism.ProjectOrganismID);

                //Delete existing Daps
                foreach (Dap item in _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetail.TreeDetailsID).ToList())
                {
                    _DatabaseContext.Daps.DeleteObject(item);
                }

                if (varas != 0) // Is Cepa
                {
                    treeDetail.Dap = 0;
                    treeDetail.Dap_Counter = 0;
                }
                else if (daps.Length == 1) // Solo un Dap
                {
                    treeDetail.Dap = Convert.ToDecimal(daps[0]);
                    treeDetail.Dap_Counter = 1;

                    Dap dapObj = new Dap();
                    dapObj.DapValue = Convert.ToDecimal(daps[0]);
                    dapObj.TreeDetailsID = treeDetail.TreeDetailsID;
                    dapObj.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                    _DatabaseContext.Daps.AddObject(dapObj);
                }
                else // 1 o Mas de un Dap
                {
                    double dapTotal = 0;
                    foreach (int dap in daps)
                    {
                        dapTotal += Convert.ToDouble(dap);
                    }
                    //dapTotal;// new DapBLL().GetDap(0); 
                    treeDetail.Dap = Convert.ToDecimal(Math.Round(Math.Sqrt(Math.Pow(dapTotal, 2D) / Convert.ToDouble(daps.Count())) * 100) / 100);
                    treeDetail.Dap_Counter = Convert.ToInt32(daps.Length); //new DapBLL().GetDapCount(0);

                    foreach (int dapInt in daps)
                    {
                        Dap dap = new Dap();
                        dap.DapValue = Convert.ToDecimal(dapInt);
                        dap.TreeDetailsID = treeDetail.TreeDetailsID;
                        dap.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                        _DatabaseContext.Daps.AddObject(dap);
                    }
                }

                //treeDetail.Dap = new DapBLL().GetDap(treeDetail.TreeDetailsID); //Convert.ToDecimal(hfDap.Value);
                //treeDetail.Dap_Counter = new DapBLL().GetDapCount(treeDetail.TreeDetailsID); //Convert.ToInt32(hfDapCounter.Value);

                treeDetail.Varas = projectOrganismUpdate.Varas;

                treeDetail.Height = Convert.ToDecimal(projectOrganismUpdate.Height);
                int CommentaryMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["CommentaryMaxLength"]);
                if (!string.IsNullOrEmpty(projectOrganismUpdate.Commentary))
                    treeDetail.Commentary = (projectOrganismUpdate.Commentary.Length > CommentaryMaxLength) ? projectOrganismUpdate.Commentary.Substring(0, CommentaryMaxLength) : projectOrganismUpdate.Commentary;
                else
                    treeDetail.Commentary = "";

                treeDetail.Number = Convert.ToInt32(treeDetail.Number);

                treeDetail.Lat = Convert.ToDecimal(projectOrganismUpdate.Lat);
                treeDetail.Lon = Convert.ToDecimal(projectOrganismUpdate.Lon);
                Dictionary<string, object> anewpointObj = Utility.ConvertToStatePlane(projectOrganismUpdate.Lon.ToString(), projectOrganismUpdate.Lat.ToString(), @"~/Javascript/");
                treeDetail.X = Convert.ToDecimal(anewpointObj["x"]);
                treeDetail.Y = Convert.ToDecimal(anewpointObj["y"]);
                //

                treeDetail.EditedDate = DateTime.Now;
                treeDetail.EditorUserID = user.UserID;

                //projectOrganism
                Organism organism = new OrganismBLL().GetOrganismByOrganismID(Convert.ToInt32(projectOrganismUpdate.OrganismID));
                projectOrganism.OrganismID = organism.OrganismID;
                projectOrganism.OrganismReference.EntityKey = organism.EntityKey;

                //ActionProposed
                ActionProposed actionProposed = new ActionProposedBLL().GetActionProposedByActionProposedID(Convert.ToInt32(projectOrganismUpdate.ActionProposedID));
                treeDetail.ActionProposedID = actionProposed.ActionProposedID;
                treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;

                //Condition
                Condition condition = new ConditionBLL().GetConditionByConditionID(Convert.ToInt32(projectOrganismUpdate.ConditionID));
                treeDetail.ConditionID = condition.ConditionID;
                treeDetail.ConditionReference.EntityKey = condition.EntityKey;

                treeDetail.ProjectOrganismID = (int)projectOrganismUpdate.ProjectOrganismID;
                treeDetail.Project_OrganismsReference.EntityKey = projectOrganism.EntityKey;

                _DatabaseContext.SaveChanges();
                //
            }

            var updatedProjectOrganism = new Project_OrganismsBLL().GetProject_OrganismsByProjectOrganismID((int)projectOrganismUpdate.ProjectOrganismID);

            return ProjectOrganismModel.GetProjectOrganismObject(updatedProjectOrganism);
        }

        [HttpDelete]
        public object Delete([FromBody]ProjectOrganismIDModel projectOrganismIDModel)
        {
            Project_Organisms projectOrganism = (Project_Organisms)HttpContext.Current.Items["projectOrganism"];
            HttpContext.Current.Items["project"] = projectOrganism.Project;

            new Project_OrganismsBLL().DeleteProject_OrganismByProjectOrganismID((int)projectOrganismIDModel.ProjectOrganismID);

            return this.GetAll(new ProjectIDModel());
        }

        #endregion
    }
}
