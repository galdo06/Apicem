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
    
    public partial class ScientificName
    {
        public ScientificName()
        {
            this.Organisms = new HashSet<Organism>();
        }
    
        public int ScientificNameID { get; set; }
        public string ScientificNameDesc { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.Guid CreatorUserID { get; set; }
        public System.DateTime EditedDate { get; set; }
        public System.Guid EditorUserID { get; set; }
    
        public virtual ICollection<Organism> Organisms { get; set; }
    }
}