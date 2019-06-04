using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Ws.Validations;

namespace WS.Models
{

    public class ProjectIDModel
    {
        [ProjectIDValidationAttribute]
        public int ProjectID { get; set; }

    }


}