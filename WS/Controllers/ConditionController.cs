using Comments.Web.Filter;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Ws.Validations;
using WS.Authorizers;
using WS.Models;

namespace WS.Controllers
{
    #region ModelHelpers

    public class ConditionIDModel
    {
        [ConditionIDValidation]
        public int? ConditionID { get; set; }
    }

    #endregion

    [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
    public class ConditionController : ApiController
    {
        public object Get(ConditionIDModel conditionIDModel)
        {
             var condition = (Condition)HttpContext.Current.Items["condition"];

            return ConditionModel.GetConditionObject(condition);
        }

        public List<object> GetAll()
        {
            return new ConditionBLL().GetAllConditions().Select(instance => ConditionModel.GetConditionObject(instance)).ToList();
        }

    }
}