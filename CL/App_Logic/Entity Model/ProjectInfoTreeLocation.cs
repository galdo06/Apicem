//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CL.App_Logic.Entity_Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectInfoTreeLocation
    {
        public int ProjectInfoTreeLocationID { get; set; }
        public int ProjectID { get; set; }
        public Nullable<decimal> Y { get; set; }
        public Nullable<decimal> X { get; set; }
        public Nullable<decimal> Lat { get; set; }
        public Nullable<decimal> Lon { get; set; }
        public Nullable<int> Parkings { get; set; }
        public Nullable<decimal> Acres { get; set; }
        public Nullable<int> DistanceBetweenTrees { get; set; }
        public Nullable<int> Lots0 { get; set; }
        public Nullable<int> Lots1 { get; set; }
        public Nullable<int> Lots2 { get; set; }
        public Nullable<int> Lots3 { get; set; }
        public Nullable<bool> SocialInterest { get; set; }
        public Nullable<int> Mitigation { get; set; }
        public Nullable<bool> PreviouslyImpacted { get; set; }
    
        public virtual Project Project { get; set; }
    }
}