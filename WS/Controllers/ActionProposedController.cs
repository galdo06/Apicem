using Comments.Web.Filter;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Ws.Validations;
using WS.Authorizers;
using WS.Models;

namespace WS.Controllers
{
    #region ModelHelpers

    public class ActionProposedIDModel
    {
        [ActionProposedIDValidation]
        public int? ActionProposedID { get; set; }
    }

    #endregion

    [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
    public class ActionProposedController : ApiController
    {
        [HttpGet]
        public object Get(ActionProposedIDModel actionProposedIDModel)
        {
            var actionProposed = (ActionProposed)HttpContext.Current.Items["actionProposed"];

            return ActionProposedModel.GetActionProposedObject(actionProposed);
        }

        [HttpGet]
        public List<object> GetAll()
        {
            return new ActionProposedBLL().GetAllActionProposeds().Select(instance => ActionProposedModel.GetActionProposedObject(instance)).ToList();
        }

    }
}