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
    public partial class CommonNameBLL
    {
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<CommonName> GetCommonNameByFilter(string commonName = default(string), string orderBy = default(string), int startRowIndex = 0, int maximumRows = 1)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "commonNameDesc";

            if (startRowIndex < 0)
                throw (new ArgumentOutOfRangeException("startRowIndex"));

            if (maximumRows < 0)
                throw (new ArgumentOutOfRangeException("maximumRows"));

            List<CommonName> commonNames = (
                    from commonNameList in
                        _DatabaseContext.CommonNames
                        .DynamicOrderBy(orderBy)
                    where
                       (string.IsNullOrEmpty(commonName) || commonNameList.CommonNameDesc.ToUpper().Trim().Contains(commonName.ToUpper().Trim()))
                    select commonNameList
                )
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList();

            return commonNames;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public CommonName GetCommonNameByCommonName(string CommonName = default(string))
        {
            return _DatabaseContext.CommonNames.FirstOrDefault(instance =>
                     instance.CommonNameDesc.Trim().ToUpper() == CommonName.Trim().ToUpper());
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public CommonName GetOrCreateCommonName(string commonNameDesc, User user)
        {
            CommonName commonName = new CommonNameBLL().GetCommonNameByCommonName(commonNameDesc);
            if (commonName == null)
            {
                commonName = new CommonName();
                commonName.CommonNameDesc = commonNameDesc;
                commonName.CreatedDate = DateTime.Now;
                commonName.CreatorUserID = user.UserID;
                commonName.EditedDate = DateTime.Now;
                commonName.EditorUserID = user.UserID;
                int commonNameID = new CommonNameBLL().CreateNewCommonName(commonName);
            }

            return commonName;
        }

    }
}
