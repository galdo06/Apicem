/****************** Copyright Notice *****************
 
This code is licensed under Microsoft Public License (Ms-PL). 
You are free to use, modify and distribute any portion of this code. 
The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:

=======================================================
   
Architecture Designed and Implemented By:
Mohammad Ashraful Alam
Microsoft Most Valuable Professional, ASP.NET 2007 � 2011
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
    public partial class RepBLL
    {

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<Rep> GetAllReps(string orderBy = default(string), int startRowIndex = default(int), int maximumRows = default(int))
        {
            return _DatabaseContext.Reps.DynamicOrderBy("RepID").ToList();
        }

    }
}
