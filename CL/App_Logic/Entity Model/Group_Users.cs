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
    
    public partial class Group_Users
    {
        public int GroupUserID { get; set; }
        public int GroupID { get; set; }
        public System.Guid UserID { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
