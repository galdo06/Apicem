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
    
    public partial class Role_Users
    {
        public int RoleUserID { get; set; }
        public int RoleID { get; set; }
        public System.Guid UserID { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
