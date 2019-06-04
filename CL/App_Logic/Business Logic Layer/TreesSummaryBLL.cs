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
    public partial class TreesSummaryBLL
    {
        public List<TreesSummary> GetItems(List<Project_Organisms> project_Organisms, int actionProposedID, bool excludeDesconicidos)
        {
            List<TreesSummary> items = (
                                            from projectOrganisms
                                                in project_Organisms.AsQueryable()
                                                .DynamicOrderBy("Organism.CommonName.CommonNameDesc")
                                            where (projectOrganisms.TreeDetails.First().ActionProposedID == actionProposedID || 0 == actionProposedID)
                                            && ((projectOrganisms.Organism.CommonName.CommonNameDesc.ToUpper().Trim() == "Desconocido".ToUpper().Trim() && !excludeDesconicidos) || excludeDesconicidos)
                                            group projectOrganisms
                                                by new
                                                {
                                                    projectOrganisms.Organism.ScientificName.ScientificNameDesc,
                                                    projectOrganisms.Organism.CommonName.CommonNameDesc
                                                }
                                                into grouped
                                                select new
                                                                    TreesSummary(grouped.Key.ScientificNameDesc, grouped.Key.CommonNameDesc, grouped.Count())
                                        ).ToList();
            return items;
        }

    }
}
