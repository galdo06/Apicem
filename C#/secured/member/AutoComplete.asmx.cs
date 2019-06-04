using Eisk.BusinessLogicLayer;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Services;
using System.Web;

namespace Eisk.Web
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : WebService
    {
        public AutoComplete()
        {
        }

        [WebMethod]
        public string[] GetCommonNameByFilter(string prefixText, int count)
        {
            return new CommonNameBLL().GetCommonNameByFilter(prefixText, default(string), 0, count).Select(instance => instance.CommonNameDesc).ToArray();
        }

        [WebMethod]
        public string[] GetScientificNameByFilter(string prefixText, int count)
        {
            return new ScientificNameBLL().GetScientificNameByFilter(prefixText, default(string), 0, count).Select(instance => instance.ScientificNameDesc).ToArray();
        }


        [WebMethod]
        public string[] GetOrganismByFilter(string prefixText, int count)
        {
            Eisk.BusinessEntities.User user = new UserBLL().GetUserByUserName((HttpContext.Current.User.Identity).Name);
            List<Eisk.BusinessEntities.Group> groups = user.Group_Users.Select(instance => instance.Group).ToList();

            List<Eisk.BusinessEntities.Organism> organisms = new OrganismBLL().GetOrganismByFilter2(prefixText, default(string), 0, count, 0, groups[0].GroupID).ToList();

            List<string> items = new List<string>();
            for (int i = 0; i < organisms.Count; i++)
            {
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(organisms[i].CommonName.CommonNameDesc.ToString() + "  -  " + organisms[i].ScientificName.ScientificNameDesc.ToString(), organisms[i].OrganismID.ToString()));
            }
            return items.ToArray();
        }

    }

}
