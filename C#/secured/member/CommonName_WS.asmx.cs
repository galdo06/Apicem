
using Eisk.BusinessLogicLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Linq;

namespace Eisk.Web.App_Logic.Helpers.Data_Access_Helpers
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class CommonName_WS : System.Web.Services.WebService
    {
        public CommonName_WS()
        {
        }

        [WebMethod]
        public string[] GetCommonNameByFilter(string CommonName)
        {
            return new CommonNameBLL().GetCommonNameByFilter(CommonName).Select(instance => instance.CommonNameDesc).ToArray();

        }
    }
}
