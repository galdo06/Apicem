using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.ComponentModel.DataAnnotations;

namespace WS.Models
{
    public class OrganismModel
    {

        public static object GetOrganismObject(Organism organism)
        {
            return (object)new
            {
                organism.OrganismID,
                organism.CommonName.CommonNameID,
                organism.CommonName.CommonNameDesc,
                organism.ScientificName.ScientificNameID,
                organism.ScientificName.ScientificNameDesc
            };
        }

        public static bool IsNull(Organism requestedOrganism, out Organism organism)
        {
            bool isNull = (requestedOrganism == null || requestedOrganism == new Organism());
            organism = isNull ? null : requestedOrganism;
            return isNull;
        }

    }
}