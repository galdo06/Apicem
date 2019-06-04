using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.Serialization;
using Ws.Validations;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WS.Models
{
    public class ProjectOrganismModel
    {
        #region Properties

        [OrganismIDValidationAttribute]
        public int? OrganismID { get; set; }

        [ProjectIDValidationAttribute]
        public int? ProjectID { get; set; }

        [LatRangeValidationAttribute]
        public decimal Lat { get; set; }

        [LonRangeValidationAttribute]
        public decimal Lon { get; set; }

        [HeightRangeValidationAttribute]
        public decimal Height { get; set; }

        [VarasCountValidationAttribute]
        public int Varas { get; set; }

        [DapsValidationAttribute]
        //public DapModel[] Daps { get; set; }
        public string Daps { get; set; }

        [ConditionIDValidationAttribute]
        public int? ConditionID { get; set; }

        [ActionProposedIDValidationAttribute]
        public int? ActionProposedID { get; set; }

        [CommentaryLengthValidationAttribute]
        public string Commentary { get; set; }

        #endregion

        #region Logical Methods

        public static bool IsAuthorized(Project_Organisms project_Organisms, List<Group> groups)
        {
            return groups.Contains(project_Organisms.Project.Group_Projects.First().Group);
        }

        public static object GetProjectOrganismObject(Project_Organisms organism)
        {
            TreeDetail treeDetail = organism.TreeDetails.First();
            return GetProjectOrganismObject(treeDetail);
        }

        public static object GetProjectOrganismObject(TreeDetail treeDetail)
        {
            var daps = treeDetail.Daps.Select(i => new { DapValue = i.DapValue });

            JObject jo = JObject.FromObject(new
            {
                Daps = daps
            }
            );

            string Daps = jo.ToString();

            var tree = new
            {
                treeDetail.ActionProposed.ActionProposedDesc,
                treeDetail.ActionProposed.ActionProposedID,
                treeDetail.ActionProposed.Color.ColorDesc,
                treeDetail.ActionProposed.Color.ColorID,
                treeDetail.ActionProposed.Color.Code,
                treeDetail.Commentary,
                treeDetail.Condition.ConditionDesc,
                treeDetail.Condition.ConditionID,
                treeDetail.CreatedDate,
                treeDetail.CreatorUserID,
                treeDetail.Dap,
                treeDetail.Dap_Counter,
                Daps,//jsonSerialiser.Serialize(treeDetail.Daps.ToList()),
                treeDetail.EditedDate,
                treeDetail.EditorUserID,
                treeDetail.Height,
                treeDetail.Lat,
                treeDetail.Lon,
                treeDetail.X,
                treeDetail.Y,
                treeDetail.Number,
                treeDetail.ProjectOrganismID,
                treeDetail.Project_Organisms.ProjectID,
                treeDetail.Project_Organisms.OrganismID,
                treeDetail.Project_Organisms.Organism.CommonName.CommonNameDesc,
                treeDetail.Project_Organisms.Organism.CommonName.CommonNameID,
                treeDetail.Project_Organisms.Organism.ScientificName.ScientificNameDesc,
                treeDetail.Project_Organisms.Organism.ScientificName.ScientificNameID,
                treeDetail.TreeDetailsID,
                treeDetail.Varas
            };

            return tree;
        }

        public static bool IsNull(Project_Organisms requestedProjectOrganism, out Project_Organisms organism)
        {
            bool isNull = (requestedProjectOrganism == null || requestedProjectOrganism == new Project_Organisms());
            organism = isNull ? null : requestedProjectOrganism;
            return isNull;
        }

        #endregion
    }
}