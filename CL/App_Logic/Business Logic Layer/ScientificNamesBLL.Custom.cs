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
using System.Web;

namespace Eisk.BusinessLogicLayer
{
    public partial class ScientificNameBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<ScientificName> GetScientificNameByFilter(string scientificName = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "scientificNameDesc";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<ScientificName> scientificNames = (
                    from scientificNameList in
                        _DatabaseContext.ScientificNames
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(scientificName) || scientificNameList.ScientificNameDesc.ToUpper().Trim().Contains(scientificName.ToUpper().Trim()))
                    select scientificNameList
                )
                .ToList();

            var scientificNamesList = scientificNames.Skip(startRowIndex).Take(maximumRows).ToList();

            HttpContext.Current.Items["scientificNames_Count"] = scientificNames.Count;

            return scientificNamesList;
        }

        public int GetTotalCountForAllScientificNames(string scientificName = default(string), string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            var count = (int)HttpContext.Current.Items["scientificNames_Count"];
            HttpContext.Current.Items.Remove("scientificNames_Count");
            return count;
            // Codigo comentado al utilizar el HttpContext
            /*
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "scientificNameDesc";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<ScientificName> scientificNames = (
                    from scientificNameList in
                        _DatabaseContext.ScientificNames
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(scientificName) || scientificNameList.ScientificNameDesc.ToUpper().Contains(scientificName.ToUpper()))
                    select scientificNameList
                )
                .ToList();
            return scientificNames.Count();
            */
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public ScientificName GetScientificNameByScientificName(string ScientificName = default(string))
        {
            return _DatabaseContext.ScientificNames.FirstOrDefault(instance =>
                     instance.ScientificNameDesc.Trim() == ScientificName);
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public   ScientificName GetScientificNameByScientificNameId2(int scientificNameID)
        {
            //Validate Input
            if (scientificNameID.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("scientificNameID");
            ScientificName scientificName = (_DatabaseContext.ScientificNames.FirstOrDefault(instance => instance.ScientificNameID == scientificNameID));

           // var ass = scientificName.ScientificNameCreator.UserName;
            return scientificName;
        }

    }
}
