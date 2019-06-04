using Eisk.BusinessEntities;
using Eisk.BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace WS.Models
{
    [DataContract]
    public class DapModel
    {
        [DataMember(IsRequired = true)] 
        public int DapValue { get; set; }
    }
}