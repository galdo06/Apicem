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
    public class ProjectOrganismUpdateModel
    {
        #region Properties

        [ProjectOrganismIDValidationAttribute]
        public int? ProjectOrganismID { get; set; }

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

    }
}