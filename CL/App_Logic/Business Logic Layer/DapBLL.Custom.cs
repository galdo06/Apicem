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
    public partial class DapBLL
    {

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public decimal GetDap(int treeDetailID)
        {
            List<Dap> daps = _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetailID).ToList();

            double total = 0;
            foreach (Dap dap in daps)
            {
                total += Convert.ToDouble(dap.DapValue);
            }

            if (total == 0)
                return 0;
            else
                return Convert.ToDecimal(Math.Round(Math.Sqrt(Math.Pow(total, 2D) / Convert.ToDouble(daps.Count)) * 100) / 100);
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public int GetDapCount(int treeDetailID)
        {
            return _DatabaseContext.Daps.Where(instance => instance.TreeDetailsID == treeDetailID).Count();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public bool DapMatch(int treeDetailID, decimal dap)
        {
            return dap == GetDap(treeDetailID);
        }

    }
}
