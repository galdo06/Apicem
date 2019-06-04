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


namespace Ws.Controllers
{
    #region ModelHelpers

    public class OrganismIDModel
    {
        [OrganismIDValidation]
        public int? OrganismID { get; set; }
    }

    #endregion

    [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
    public class OrganismController : ApiController
    {
        #region Api Calls

        [HttpGet]
        public object Get(OrganismIDModel organismIDModel)
        {
            var organism = (Organism)HttpContext.Current.Items["organism"];

            return OrganismModel.GetOrganismObject(organism);
        }

        [HttpGet]
        public List<object> GetAll()
        {
            return new OrganismBLL().GetAllOrganisms().Select(instance => OrganismModel.GetOrganismObject(instance)).ToList();
        }

        [HttpGet]
        public List<object> GetAllTrees()
        {
                        User user = (User)HttpContext.Current.Items["user"];
          
            //Solo arboles
                        return new OrganismBLL().GetOrganismByFilter2(null, null, 0, 10000, 2, user.Group_Users.First().GroupID)
            .Where(i => i.OrganismTypeID == 2).Select(instance => OrganismModel.GetOrganismObject(instance)).ToList();
        }

        #endregion
    }
}
