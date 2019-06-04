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
using CL.JavaScript;
using System.Web;

namespace Eisk.BusinessLogicLayer
{
    public partial class TreeDetailBLL
    {
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public int GetTotalCountForAllTreeDetailsInProjectByFilter(string treeDetail = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int projectID = 0, int actionProposed = 0, int condition = 0, bool isReverse = false, bool isCepas = false, bool isLittoral = false, bool isMaritimeZone = false, int actionProposedChange = 0)
        {
            var count = (int)HttpContext.Current.Items["treeDetails_Count"];
            HttpContext.Current.Items.Remove("treeDetails_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
            //if (string.IsNullOrEmpty(orderBy))
            //    orderBy = "editedDate desc";
            int treeDetailINT = 0;
            bool isINT = int.TryParse(treeDetail, out treeDetailINT);

            List<TreeDetail> trees = (
                from groupTreeDetailsList in
                    _DatabaseContext.TreeDetails
                where
                (groupTreeDetailsList.Project_Organisms.ProjectID == projectID)
                select (groupTreeDetailsList)
            )
            .Where
            (
                instance => (
                                (
                                    string.IsNullOrEmpty(treeDetail)
                                    ||
                                    (
                                        instance.Project_Organisms.Organism.CommonName.CommonNameDesc.ToUpper().Trim().Contains(treeDetail.ToUpper().Trim())
                                    ||
                                        instance.Project_Organisms.Organism.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(treeDetail.ToUpper().Trim())
                                    ||
                                        (treeDetailINT != 0 && instance.Varas == treeDetailINT)
                                    ||
                                        (treeDetailINT != 0 && instance.Dap == treeDetailINT)
                                    ||
                                        (treeDetailINT != 0 && instance.Height == treeDetailINT)
                                    ||
                                        instance.Commentary.ToUpper().Trim().Contains(treeDetail.ToUpper().Trim())
                                    )
                                )
                                &&
                                (actionProposed == 0 || instance.ActionProposedID == actionProposed)
                                &&
                                (!isCepas || instance.Varas > 0)
                        )
            )
            .ToList();

            return trees.Count;
             */

        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<TreeDetail> GetTreeDetailsInProjectByFilter(string treeDetail = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1, int projectID = 0, int actionProposed = 0, int condition = 0, bool isReverse = false, bool isCepas = false, bool isLittoral = false, bool isMaritimeZone = false, int actionProposedChange = 0)
        {

            orderBy =
                (string.IsNullOrEmpty(orderBy) ? " Number" : orderBy) +
                ((isReverse) ? " DESC" : "") +
                 ", Number" +
                ((isReverse) ? " DESC" : "");

            int treeDetailINT = 0;
            bool isINT = int.TryParse(treeDetail, out treeDetailINT);

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var treeDetails = (
                from groupTreeDetailsList in
                    _DatabaseContext.TreeDetails
                    .DynamicOrderBy(orderBy)
                where
                (groupTreeDetailsList.Project_Organisms.ProjectID == projectID)
                select (groupTreeDetailsList)
            )
            .Where
            (
                instance => (
                                (
                                    string.IsNullOrEmpty(treeDetail)
                                    ||
                                    (
                                        instance.Project_Organisms.Organism.CommonName.CommonNameDesc.ToUpper().Trim().Contains(treeDetail.ToUpper().Trim())
                                    ||
                                        instance.Project_Organisms.Organism.ScientificName.ScientificNameDesc.ToUpper().Trim().Contains(treeDetail.ToUpper().Trim())
                                    ||
                                        (treeDetailINT != 0 && instance.Varas == treeDetailINT)
                                    ||
                                        (treeDetailINT != 0 && instance.Dap == treeDetailINT)
                                    ||
                                        (treeDetailINT != 0 && instance.Height == treeDetailINT)
                                    ||
                                        instance.Commentary.ToUpper().Trim().Contains(treeDetail.ToUpper().Trim())
                                    )
                                )
                                &&
                                (actionProposed == 0 || instance.ActionProposedID == actionProposed)
                                &&
                                (condition == 0 || instance.ConditionID == condition)                                
                                &&
                                (!isCepas || instance.Varas > 0)
                                &&
                                (!isLittoral || instance.Littoral == isLittoral)
                                &&
                                (!isMaritimeZone || instance.MaritimeZone == isMaritimeZone)
                        )
            );

            var returnTreeDetailsList = treeDetails.Skip(startRowIndex).Take(maximumRows).ToList();
            if (returnTreeDetailsList.Count > 0)
            {
                foreach (TreeDetail treeDetail2 in returnTreeDetailsList)
                {
                    var commonNameDesc = treeDetail2.Project_Organisms.Organism.CommonName.CommonNameDesc;
                    var scientificNameDesc = treeDetail2.Project_Organisms.Organism.ScientificName.ScientificNameDesc;
                    var actionProposedDesc = treeDetail2.ActionProposed.ActionProposedDesc;
                    var conditionDesc = treeDetail2.Condition.ConditionDesc;
                }
            }

            HttpContext.Current.Items["treeDetails_Count"] = treeDetails.Count();

            return returnTreeDetailsList;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<TreeDetail> GetTreeDetailsByProjectID(int projectID = 0)
        {

            List<TreeDetail> treeDetails = (
                from groupTreeDetailsList in
                    _DatabaseContext.TreeDetails
                where
                (groupTreeDetailsList.Project_Organisms.ProjectID == projectID)
                select (groupTreeDetailsList)
            )
            .ToList();

            if (treeDetails.Count > 0)
            {
                foreach (TreeDetail treeDetail3 in treeDetails)
                {
                    var commonNameDesc = treeDetail3.Project_Organisms.Organism.CommonName.CommonNameDesc;
                    var scientificNameDesc = treeDetail3.Project_Organisms.Organism.ScientificName.ScientificNameDesc;
                    var actionProposedDesc = treeDetail3.ActionProposed.ActionProposedDesc;
                }
            }

            return treeDetails;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------

        public TreeDetail CreateProject_Organism(int varas, decimal height, decimal lat, decimal lon, int[] daps, ActionProposed actionProposed, Condition condition, string commentary, Organism organism, Project project, Eisk.BusinessEntities.User user)
        {

            Project_Organisms project_Organism = new Project_Organisms();

            project_Organism.CreatorUserID = user.UserID;
            project_Organism.CreatedDate = DateTime.Now;
            project_Organism.EditorUserID = user.UserID;
            project_Organism.EditedDate = DateTime.Now;

            project_Organism.OrganismID = organism.OrganismID;
            project_Organism.OrganismReference.EntityKey = organism.EntityKey;

            project_Organism.ProjectID = project.ProjectID;
            project_Organism.ProjectReference.EntityKey = project.EntityKey;

            int project_OrganismID = new Project_OrganismsBLL().CreateNewProject_Organisms(project_Organism);

            TreeDetail treeDetail = new TreeDetail();

            if (varas != 0) // Is Cepa
            {
                treeDetail.Dap = 0;
                treeDetail.Dap_Counter = 0;
            }
            else if (daps.Length == 1) // Solo un Dap
            {
                treeDetail.Dap = daps[0];
                treeDetail.Dap_Counter = 1;
            }
            else // Mas de un Dap
            {
                double dapTotal = 0;
                foreach (int dap in daps)
                {
                    dapTotal += dap;
                }
                treeDetail.Dap = Convert.ToDecimal(Math.Round(Math.Sqrt(Math.Pow(dapTotal, 2D) / Convert.ToDouble(daps.Length)) * 100) / 100);
                treeDetail.Dap_Counter = daps.Length;
            }

            treeDetail.Varas = varas;

            treeDetail.Height = height;
            treeDetail.Commentary = commentary;

            List<Project_Organisms> project_Organisms = new Project_OrganismsBLL().GetProject_OrganismsByProjectID(project.ProjectID);

            Dictionary<string, object> anewpointObj = JavaScriptHelper.ConvertToStatePlane(lon.ToString(), lat.ToString());

            treeDetail.Number = project_Organisms.Count;
            treeDetail.Y = Convert.ToDecimal(anewpointObj["y"]);
            treeDetail.X = Convert.ToDecimal(anewpointObj["x"]);
            treeDetail.Lat = Convert.ToDecimal(lat);
            treeDetail.Lon = Convert.ToDecimal(lon);

            treeDetail.CreatedDate = DateTime.Now;
            treeDetail.CreatorUserID = user.UserID;
            treeDetail.EditedDate = DateTime.Now;
            treeDetail.EditorUserID = user.UserID;


            treeDetail.ActionProposedID = actionProposed.ActionProposedID;
            treeDetail.ActionProposedReference.EntityKey = actionProposed.EntityKey;

            treeDetail.ConditionID = condition.ConditionID;
            treeDetail.ConditionReference.EntityKey = condition.EntityKey;

            treeDetail.ProjectOrganismID = project_Organism.ProjectOrganismID;
            treeDetail.Project_OrganismsReference.EntityKey = project_Organism.EntityKey;

            int treeDetailID = new TreeDetailBLL().CreateNewTreeDetail(treeDetail);

            foreach (decimal dapDecimal in daps)
            {
                Dap dap = new Dap();
                dap.DapValue = dapDecimal;
                dap.TreeDetailsID = treeDetailID;
                dap.TreeDetailReference.EntityKey = treeDetail.EntityKey;

                new DapBLL().CreateNewDap(dap);
            }
            return treeDetail;
        }

    }
}
