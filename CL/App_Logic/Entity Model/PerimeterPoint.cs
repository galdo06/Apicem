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
    
    public partial class PerimeterPoint
    {
        public int PerimeterPointID { get; set; }
        public int PerimeterID { get; set; }
        public decimal Y { get; set; }
        public decimal X { get; set; }
    
        public virtual Perimeter Perimeter { get; set; }
    }
}
