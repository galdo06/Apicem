using Comments.Web.Filter;
using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Web.Http;
using WS.Authorizers;
using System;
using System.ComponentModel.DataAnnotations;
using Ws.Validations;
using System.Web;
using WS.Models;

namespace WS.Controllers
{
    #region ModelHelpers

    public class CityIDModel
    {
        [CityIDValidation]
        public int? CityID { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CityIDValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int cityID;
            City city;
            if (value == null)
            {
                this.ErrorMessage = "CityID es requerido";
                return false;
            }

            if (!int.TryParse(value.ToString(), out cityID))
            {
                this.ErrorMessage = "CityID contiene formato inválido";
                return false;
            }

            if (cityID == 0 || CityModel.IsNull(new CityBLL().GetCityByCityID(cityID), out city))
            {
                this.ErrorMessage = "CityID no existente";
                return false;
            }

            HttpContext.Current.Items["city"] = city;

            return true;

        }
    }

    #endregion

    [AccessAuthorize("IsAuthorizedRetreiveData", typeof(AccessAuthorizer), Roles = "Administrator,User")]
    public class CityController : ApiController
    {   

        [HttpGet]
        public object Get(CityIDModel cityIDModel)
        {
           var  city=  (City)HttpContext.Current.Items["city"];

           return CityModel.GetCityObject(city); 
        }

        [HttpGet]
        public List<object> GetAll()
        {
            return new CityBLL().GetAllCities().Select(instance => CityModel.GetCityObject(instance)).ToList();
        }
    }
}