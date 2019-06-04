using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.ComponentModel.DataAnnotations;

namespace WS.Models
{
    public static class CityModel
    {
        public static bool ValidateCityID(int cityID, out City city)
        {
            city = new CityBLL().GetCityByCityID(cityID);

            return (city != null && city != new City());
        }

        public static object GetCityObject(City city)
        {
            return (object)new
            {
                city.CityName,
                city.CityID,
                city.X,
                city.Y
            };
        }

        public static bool IsNull(City requestedCity, out City city)
        {
            bool isNull = (requestedCity == null || requestedCity == new City());
            city = isNull ? null : requestedCity;
            return isNull;
        }

    }
}