/****************** Copyright Notice *****************
 
This code is licensed under Microsoft Public License (Ms-PL). 
You are free to use, modify and distribute any portion of this code. 
The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:

=======================================================
   
Architecture Designed and Implemented By:
Mohammad Ashraful Alam
Microsoft Most Valuable Professional, ASP.NET 2007 – 2011
Twitter: http://twitter.com/AshrafulAlam | Blog: http://blog.ashraful.net | Portfolio: http://www.ashraful.net
   
*******************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;

namespace Eisk.BusinessLogicLayer
{
    public partial class OrganismTypeBLL
    {
        public List<OrganismType> GetAllOrganismTypesList()
        {
            List<OrganismType> organismTypes = _DatabaseContext.OrganismTypes.DynamicOrderBy("organismTypeName").ToList();

            return organismTypes;
        }
        public List<OrganismType> GetOrganismTypesListFiltered(int organismTypeID)
        {
            List<OrganismType> organismTypes = _DatabaseContext.OrganismTypes.DynamicOrderBy("organismTypeName").Where(instance => instance.OrganismTypeID == organismTypeID).ToList();

            return organismTypes;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public OrganismType GetOrganismTyeByOrganismTypeId2(int organismTypeID)
        {
            //Validate Input
            if (organismTypeID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("organismTyepID");
            OrganismType organismType = (_DatabaseContext.OrganismTypes.FirstOrDefault(instance => instance.OrganismTypeID == organismTypeID));

            var ass = organismType.OrganismTypeName;
            return organismType;
        }

        public List<OrganismType> GetOrganismTypeByFilter(string organismType = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "organismTypeName";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            var organismTypesVar = (
                from organismTypes in
                    _DatabaseContext.OrganismTypes
                    .DynamicOrderBy(orderBy)
                where
                       (string.IsNullOrEmpty(organismType) || organismTypes.OrganismTypeName.ToUpper().Trim().Contains(organismType.ToUpper().Trim()))
                select (organismTypes)
            )
            .Skip(startRowIndex)
            .Take(maximumRows);

            List<OrganismType> organismTypesList = organismTypesVar.ToList();
            if (organismTypesList.Count > 0)
            {
                var x = organismTypesList[0].OrganismTypeName;
            }
            return organismTypesList;
        }

        public int GetTotalCountForAllOrganismTypes(string organismType = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            var organismTypesVar = (
                from organismTypes in
                    _DatabaseContext.OrganismTypes
                where
                      (string.IsNullOrEmpty(organismType) || organismTypes.OrganismTypeName.ToUpper().Trim().Contains(organismType.ToUpper().Trim()))
              select (organismTypes)
            );

            List<OrganismType> organismTypesList = organismTypesVar.ToList();
            if (organismTypesList.Count > 0)
            {
                var x = organismTypesList[0].OrganismTypeName;
            }
            return organismTypesList.Count();
        }

    }
}
