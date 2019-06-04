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
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Eisk.BusinessEntities;
using Eisk.Helpers;

namespace Eisk.BusinessLogicLayer
{
    public partial class ApplicationBLL
    {

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public Application GetApplicationByApplicationName(string applicationName)
        {
            //Validate Input
            if (applicationName.IsInvalidKey())
                BusinessLayerHelper.ThrowErrorForInvalidDataKey("applicationID");
            return (_DatabaseContext.Applications.FirstOrDefault(application => application.ApplicationName == applicationName));
        }
			
    }// Custom comparer for the Product class 
    class ApplicationComparer : IEqualityComparer<Application>
    {
        // Products are equal if their names and product numbers are equal. 
        public bool Equals(Application applicationA, Application applicationB)
        {

            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(applicationA, applicationB)) return true;

            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(applicationA, null) || Object.ReferenceEquals(applicationB, null))
                return false;

            //Check whether the products' properties are equal. 
            return applicationA.ApplicationName == applicationB.ApplicationName && applicationA.ApplicationID == applicationB.ApplicationID;
        }
    
        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 

        public int GetHashCode(Application application)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(application, null)) return 0;

            //Get hash code for the Name field if it is not null. 
            int hashApplicationName = application.ApplicationName == null ? 0 : application.ApplicationName.GetHashCode();

            //Get hash code for the ID field. 
            int hashApplicationID = application.ApplicationID.GetHashCode();

            //Calculate the hash code for the product. 
            return hashApplicationName ^ hashApplicationID;
        }

    }
}
